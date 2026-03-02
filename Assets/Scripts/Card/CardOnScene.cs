using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Proto2.Enums;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;

public class CardOnScene : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    #region field
    [Header("Own Field")]
    [SerializeField] private CardData data;
    [SerializeField] private bool isPlayable = true;
    [SerializeField] private Vector3 originalTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CharacterOnScene owner;

    [Header("Visual UI Field")]
    [SerializeField] private Image myImage;
    [SerializeField] private TMP_Text[] textList;
    [SerializeField] private Text[] conditionList;

    [Header("Raycast Radius")]
    [SerializeField] private float radius = 0.25f;

    [Header("For Debug")]
    public int handIndex;
    public GameObject target;
    public CardInstance cardInstance;

    #endregion
    
    #region methods
    public void Awake()
    {
        //비주얼 컴포넌트들 연결
        mainCamera = Camera.main;
        originalTransform = transform.position;
        canvas = GetComponentInChildren<Canvas>();
        myImage = canvas.GetComponentInChildren<Image>();
        textList = canvas.GetComponentsInChildren<TMP_Text>();  //카드 이름, 카드 텍스트 순으로 가져온다.
        conditionList = canvas.GetComponentsInChildren<Text>(); //ATK, DEF, SPD 순으로 가져온다.
        // How to Search Owner Character of this Card?
   
    }

    public void SetCard(CardData cardData, int index, CardInstance inst)
    {
        data = cardData;
        handIndex = index;
        cardInstance = inst;

        //비주얼 세팅
        myImage.sprite =  data.CardSprite;
        textList[0].text = data.CardName;
        textList[1].text = data.CardText;

        //조건 텍스트 세팅
        foreach(ActiveConditionData conditionData in data.ActiveConditionList)
        {
            switch (conditionData.Condition)
            {
                case ConditionType.Attack:
                    {
                        conditionList[0].text = conditionData.Value.ToString();
                        break;
                    }
                case ConditionType.Shield:
                    {
                        conditionList[1].text = conditionData.Value.ToString(); 
                        break;
                    }
                case ConditionType.Speed:
                    {
                        conditionList[2].text = conditionData.Value.ToString();
                        break;
                    }
            }
        }

        //주인 찾기
        foreach(CharacterOnScene ch in BattleManager.Instance.PlayerParty)
        {
           if(ch.CharacterData.CardColor == data.Color)
            {
               owner = ch; break;
            }
           else if(data.Color == CardColor.Gray)
            {
                owner = BattleManager.Instance.TurnCharacter;
            }
        }
    }

    public void CardsizeBig()
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 1f);
    }

    public void CardsizeSmall()
    {
        transform.localScale = new Vector3(0.2f, 0.2f, 1f);
    }
    public void Use()
    {
        foreach (var actionData in data.CarActionDataList)
        {
            CardActionProcessor.GetAction(actionData.CardActionType).DoAction(new CardActionParameters(actionData.ActionValue, owner, target ? target.GetComponent<EnemyOnScene>() : null, data, this));
        }

        //AfterUsed();
        HandController.Instance.AfterCardUse(this);
    }

    public void BackToHand()
    {
        transform.position = originalTransform;
        CardsizeSmall();
        myImage.sprite = data.CardSprite;
        //Color color = Color.white;
        //color.a = 1f;
        //myImage.color = color;           OnDrag 참조

        //SetCard(data);
    }
    public bool CheckCondition(CardData myData, CharacterOnScene character)
    {
        float val; 
        float currentStat = 0f;
        bool lastCheck = false;
        
        //중립 카드 사용 시 참고할 캐릭터
        if(data.Color == CardColor.Gray)
        {
            character = BattleManager.Instance.TurnCharacter;
        }

        //턴 캐릭터 체크
        if(character != BattleManager.Instance.TurnCharacter)
        {
            Debug.Log("Not Turn Character");
            return false;
        }
        for(int i=0; i<myData.ActiveConditionList.Count; i++)
        {
            val = myData.ActiveConditionList[i].Value;

            switch (myData.ActiveConditionList[i].Condition)
            {
                case ConditionType.Attack:
                    currentStat = character.CurrentAttack;
                    break;

                case ConditionType.Speed: 
                    currentStat = character.CurrentSpeed;
                    break;

                case ConditionType.Shield: 
                    currentStat = character.CurrentShield;
                    break;
            }
            //Debug.Log("val" + val);
            //Debug.Log("currentStat" + currentStat);
            if (currentStat >= val)
            {
                lastCheck = true;
                //Debug.Log(lastCheck);
            }
            else
            {
                lastCheck = false;
                break;
                //Debug.Log(lastCheck);
            }
        }
        //Debug.Log("checked");
        return lastCheck;
    }
    public bool CheckTarget(CardData data)
    {
       
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapCircle(mouseWorldPos, radius);
        /*if (hit != null)
        {
            target = hit.gameObject;
            Debug.Log(hit.gameObject.name);
        }*/

        if (data.UsableWithoutTarget) return true;

        if (hit != null)
        {
            target = hit.gameObject;
            Debug.Log(hit.gameObject.name);
            if (data.ActionTargetType.ToString() == hit.tag)
            {

                Debug.Log("valid target");
                return true;
            }
            else
            {
                Debug.Log("invalid target");
                Debug.Log("target : " + hit.tag + ", data : " + data.ActionTargetType);
                return false;
            }
        }
        else if (hit == null)
        {
            Debug.Log("Nothing detected");
            return false;
        }

        else Debug.Log("else");  return false;
    }

    void OnDrawGizmos()
    {
        if (Camera.main == null) return;

        Vector2 mouseWroldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(mouseWroldPos, radius);
    }

    #endregion

    #region EventHandler
    public void OnPointerEnter(PointerEventData eventdata)
    {
        CardsizeBig();
    }

    public void OnPointerExit(PointerEventData eventdata)
    {
        CardsizeSmall();
    }

    public void OnBeginDrag(PointerEventData eventdata)
    {
        CardsizeSmall();
        originalTransform = transform.position;
        myImage.sprite = data.DragIcon;
    }

    public void OnDrag(PointerEventData eventdata)
    {
        //Color currentColor = Color.white;
        //currentColor.a = 0f;
        //myImage.color = currentColor;
        /* 드래그를 하는동안 카드를 투명하게 바꿔주는 부분이다.
         * UI를 투명화 한 뒤 게임뷰에서 기즈모처럼 레이캐스트 범위를 시각화해서 타겟 아이콘을 만들고 싶었다.
         * 그러나 LineRenderer 컴포넌트를 넣기 싫고 코드만으로는 어떻게 하는지 모르겠어서 그냥 이미지 변경으로 타혐했다. 
         * 그 결과 카드 귀퉁이를 잡으면 타겟 아이콘과 실제 레이캐스트 범위가 차이난다는 큰 단점이 발생했다.*/
        /*이론상 ancor stretch 상태라 offset 기반으로 움직이는게 좋지만 귀찮아서 예전에 쓰던거 그대로 가져왔다.
         *따라서 발동 조건이랑 일치하지 않아서 약간 애매모호하다.
         *나중에 핸드를 자동으로 정렬시킬 때 문제가 될 수 있음. */
        Vector3 worldPoint = mainCamera.ScreenToWorldPoint(new Vector3(eventdata.position.x, eventdata.position.y, 10f));
        transform.position = new Vector3(worldPoint.x, worldPoint.y, originalTransform.z);

    }

    public void OnEndDrag(PointerEventData eventdata)
    {
        if(transform.position.y >= -0.8f)
        {
            //Debug.Log("used");
            if(CheckCondition(data, owner))
            {
                if (CheckTarget(data))
                {
                    //Use();
                    Debug.Log("Card Used Successfull");
                    Use();
                    //BackToHand();
                }
                else BackToHand();
            }
            else BackToHand();
        }

        else
            BackToHand();
        
        Debug.Log(transform.position.y);
    }
    #endregion
}
