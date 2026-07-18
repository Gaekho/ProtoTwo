using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DialogueUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image leftImage;
    [SerializeField] private Image rightImage;
    [SerializeField] private Sprite basicTransparent;
    [SerializeField] private Color darkFade;

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
        ParseTags(tags);
        SetText(text);
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

    public void SetImage()
    {

    }
    public void ParseTags(List<string> tags)
    {
        if (tags == null || tags.Count == 0) return;

        foreach (string tag in tags)
        {
            string[] splitTag = tag.Split(':');
            if(splitTag.Length != 2) continue;

            string key = splitTag[0].Trim().ToLower();
            string value = splitTag[1].Trim();

            switch (key)
            {
                case "speaker":
                    SetSpeaker(value); break;
                case "sprite":
                    SetImage(); break;
                default:
                    Debug.Log($"Not justified Tag Key : {key}"); break;
            }
        }
    }
}
