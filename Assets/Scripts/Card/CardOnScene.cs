using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Proto2.Enums;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;

//v0.04 /2026.03.09 / 14:46
//변경 요약 : layermask 기반 충돌 감지 추가, 드래그 중 canvas 비활성화. 중립카드 사용 시도 시 주인을 턴 캐릭터로 변경.
public class CardOnScene : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    #region field
    [Header("Own Field")]
    [SerializeField] private CardData data;
    [SerializeField] private bool isPlayable = true;
    [SerializeField] private Vector3 originalTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private AllyUnit owner;

    [Header("Visual UI Field")]
    [SerializeField] private SpriteRenderer mySprite;
    [SerializeField] private TMP_Text[] textList;
    [SerializeField] private Text[] conditionList;

    [Header("Raycast")]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float radius = 0.25f;

    [Header("For Debug")]
    public int handIndex;
    public BattleUnitBase target;
    public CardInstance cardInstance;

    #endregion
    
    #region methods
    public void Awake()
    {
        //비주얼 컴포넌트들 연결
        mainCamera = Camera.main;
        originalTransform = transform.position;
        canvas = GetComponentInChildren<Canvas>();
        mySprite = GetComponent<SpriteRenderer>();
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
        mySprite.sprite =  data.CardSprite;
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
        foreach(AllyUnit ally in BattleManager.Instance.PlayerParty)
        {
           if(ally.CharacterData.CardColor == data.Color)
            {
               owner = ally; break;
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

    public IEnumerator CardUseRoutine()
    {

        switch (data.CardAnimTrigger)
        {
            case CardAnimTrigger.Attack:
                owner.DoAttackAnim();
                //yield return owner.WaitForAnimationStateEnd("Attack");
                break;

            case CardAnimTrigger.AddArmor:
                owner.DoArmorAnim();
                //yield return owner.WaitForAnimationStateEnd("AddArmor");
                break;

            case CardAnimTrigger.ApplyBuff:
                owner.DoApplyBuffAnim();
                //yield return owner.WaitForAnimationStateEnd("ApplyBuff");
                break;

            case CardAnimTrigger.ApplyDebuff:
                owner.DoApplyDebuffAnim();
                //yield return owner.WaitForAnimationStateEnd("ApplyDebuff");
                break;

            case CardAnimTrigger.Draw:
                owner.DoDrawAnim();
                //yield return owner.WaitForAnimationStateEnd("Draw");
                break;
        }
        foreach (var actionData in data.CarActionList)
        {
            actionData.DoAction(new CardActionParameters(owner, target, data, cardInstance, this));
        }

        yield return new WaitForSeconds(0.1f);
        //AfterUsed();
        HandController.Instance.AfterCardUse(this);

    }
    public void Use()
    {
        switch (data.CardAnimTrigger)
        {
            case CardAnimTrigger.Attack:
                owner.DoAttackAnim(); break;

            case CardAnimTrigger.AddArmor:
                owner.DoArmorAnim(); break;
            
            case CardAnimTrigger.ApplyBuff: 
                owner.DoApplyBuffAnim(); break;

            case CardAnimTrigger.ApplyDebuff: 
                owner.DoApplyDebuffAnim(); break;

            case CardAnimTrigger.Draw: 
                owner.DoDrawAnim(); break;
        }
        foreach (var actionData in data.CarActionList)
        {
            actionData.DoAction(new CardActionParameters(owner, target, data, cardInstance, this));
        }

        //AfterUsed();
        HandController.Instance.AfterCardUse(this);
    }

    public void BackToHand()
    {
        transform.position = originalTransform;
        CardsizeSmall();
        mySprite.sprite = data.CardSprite;
        canvas.gameObject.SetActive(true);
        //Color color = Color.white;
        //color.a = 1f;
        //myImage.color = color;           OnDrag 참조

        //SetCard(data);
    }
    public bool CheckCondition(CardData myData, AllyUnit owner)
    {
        float val; 
        float currentStat = 0f;
        bool lastCheck = false;
        
        //중립 카드 사용 시 참고할 캐릭터
        if(data.Color == CardColor.Gray)
        {
            owner = BattleManager.Instance.TurnCharacter;
            this.owner = BattleManager.Instance.TurnCharacter;
        }

        //턴 캐릭터 체크
        if(owner != BattleManager.Instance.TurnCharacter)
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
                    currentStat = owner.CurrentAttack;
                    break;

                case ConditionType.Speed: 
                    currentStat = owner.CurrentSpeed;
                    break;

                case ConditionType.Shield: 
                    currentStat = owner.CurrentShield;
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
        if (data.UsableWithoutTarget) return true;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapCircle(mouseWorldPos, radius, layerMask);

        if (hit == null) { Debug.Log("Nothing Detected"); return false; }

        target = hit.gameObject.GetComponentInParent<BattleUnitBase>();
        if (target == null) 
        { 
            Debug.Log("target is null");  
            Debug.Log($"hit = {hit.name}");
            Debug.Log($"parent battle unit = {hit.GetComponentInParent<BattleUnitBase>()}");
            Debug.Log($"self battle unit = {hit.GetComponent<BattleUnitBase>()}");
            return false; 
        }


        switch (data.CardTarget)
        {
            case CardTargetType.Ally:
                return target.Team == UnitTeam.Ally;

            case CardTargetType.Enemy:
                return target.Team == UnitTeam.Enemy;
        }

        return false;
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
        mySprite.sprite = data.DragIcon;
        canvas.gameObject.SetActive(false);
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
        if (BattleManager.Instance.IsResolving)
        {
            BackToHand();
            Debug.Log("Can't Use Card Now!");
            return;
        }

        if(transform.position.y >= -0.8f)
        {
            //Debug.Log("used");
            if(CheckCondition(data, owner))
            {
                if (CheckTarget(data))
                {
                    //Use();
                    Debug.Log("Card Used Successfull");
                    StartCoroutine(BattleManager.Instance.ResolveRoutine(CardUseRoutine()));
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

[System.Serializable]
public class CardInstance
{
    public int id;
    public CardData cardData;

    public CardInstance(int id, CardData cardData)
    {
        this.id = id;
        this.cardData = cardData;
    }
}
