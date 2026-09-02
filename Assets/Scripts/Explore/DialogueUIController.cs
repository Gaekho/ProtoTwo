using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Ink.Parsed;
public class DialogueUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image leftImage;
    [SerializeField] private Image rightImage;
    [SerializeField] private Sprite basicTransparent;
    [SerializeField] private Color darkFade;
    [SerializeField] private Button choiceButton;
    [SerializeField] private Transform choiceParent;

    private bool hasChoice = false;
    private void Update()
    {
        if (DialogueManager.Instance.DialoguePlaying() && Input.GetMouseButtonDown(0))
        {
            if (hasChoice)
            {
                return;
            }

            Debug.Log("Clicked & Advance");
            DialogueManager.Instance.AdvanceStroy();
        }
    }
    public void SetDialogueUI(string text, List<Ink.Runtime.Choice> choices, List<string> tags)
    {
        RemoveChoice();
        ParseTags(tags);
        SetText(text);

        if (choices.Count > 0)
        {
            hasChoice = true;
            Debug.Log($"count of choice : {choices.Count}");
            SetChoice(choices);
        }
    }

    public void SetSpeaker(string speakerName)
    {
        if (speakerName != null)
        {
            speakerText.text = speakerName;
        }
        else 
        { 
            speakerText.text = string.Empty; 
        }
    }

    public void SetText(string text)
    {
        if (text != null)
        {
            dialogueText.text = text;
        }
        else { dialogueText.text = string.Empty; }
    }

    public void SetImage(string spriteName)
    {
        //Sprite[] sprites = Resources.LoadAll<Sprite>("Assets/Resources/DialogueSprite/{characterName}");   // 캐릭터 이름으로 시트 찾기
        //Sprite specificSprite = System.Array.Find(sprites, x => x.name == mood);                           // 캐릭터 기분으로 상태 찾기
        leftImage.sprite = Resources.Load<Sprite>($"Assets/Resources/DialogueSprite/{spriteName}");
    }

    public void SetChoice(List<Ink.Runtime.Choice> choices)
    {
        for(int i = 0; i < choices.Count; i++)
        {
            Button newChoiceButton = Instantiate(choiceButton, choiceParent);
            
            TMP_Text buttonText = newChoiceButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = choices[i].text;

            int choiceIndex = i;
            newChoiceButton.onClick.AddListener( ()=>OnChoiceSelected(choiceIndex));
        }
    }

    private void OnChoiceSelected(int choiceIndex)
    {
        DialogueManager.Instance.SelectChoice(choiceIndex);
        hasChoice = false;
    }

    private void RemoveChoice()
    {
        foreach(Transform child in choiceParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void ParseTags(List<string> tags)
    {
        if (tags == null || tags.Count == 0) return;

        string characterName = "";
        string characterSheet = "";
        string characterMood = "";
        string characterLayout = "";

        foreach (string tag in tags)
        {
            string[] splitTag = tag.Split(':');
            if(splitTag.Length != 2) continue;

            string key = splitTag[0].Trim().ToLower();
            string value = splitTag[1].Trim();
            Debug.Log($"{key} : {value}");

            switch (key)
            {
                case "speaker":
                    characterName = value; break;
                case "sheet":
                    characterSheet = value;  break;
                case "mood":
                    characterMood = value; break;
                case "layout":
                    characterLayout = value; break;
                default:
                    Debug.Log($"Not justified Tag Key : {key}"); break;
            }
        }

        //Speaker Text Setting
        SetSpeaker(characterName);

        //Sprite specialize
        //Sprite characterSprite = Resources.Load<Sprite>("");
        Sprite[] sprites = Resources.LoadAll<Sprite>($"DialogueSprite/{characterSheet}");          // 캐릭터 이름으로 시트 찾기

        Debug.Log("Length of Sheet :" + sprites.Length);
        Sprite specificSprite = System.Array.Find(sprites, x => x.name == characterMood);                           // 캐릭터 기분으로 상태 찾기

        //Debug.Log($"{specificSprite.name}");
        if (specificSprite != null)
        {
            if(characterLayout == "left")
            {
                //대화 진행 중, 왼쪽 캐릭터 이미지 변경하는 경우.
                if(leftImage.gameObject.activeInHierarchy)
                {
                    leftImage.color = Color.white;
                    leftImage.sprite = specificSprite;

                }
                //대화 처음 시작, 왼쪽 캐릭터 발화 시
                else
                {
                    leftImage.gameObject.SetActive(true);
                    leftImage.color = Color.white;
                    leftImage.sprite = specificSprite;
                }

                //왼쪽 캐릭터 발화 시 오른쪽 캐릭터 어둡게 만들기
                if(rightImage.gameObject.activeInHierarchy)
                {
                    rightImage.color = darkFade;
                }
            }
            else if (characterLayout == "right")
            {
                //대화 진행 중, 오른쪽 캐릭터 이미지 변경하는 경우.
                if (rightImage.gameObject.activeInHierarchy)
                {
                    rightImage.color = Color.white;
                    rightImage.sprite = specificSprite;

                }
                //대화 처음 시작, 오른쪽 캐릭터 발화 시
                else
                {
                    rightImage.gameObject.SetActive(true);
                    rightImage.color = Color.white;
                    rightImage.sprite = specificSprite;
                }

                //오른쪽 캐릭터 발화 시 왼쪽 캐릭터 어둡게 만들기
                if (leftImage.gameObject.activeInHierarchy)
                {
                    leftImage.color = darkFade;
                }
            }
        }
    }
}

