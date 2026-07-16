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

            DialogueManager.Instance.AdvanceStroy();
        }
    }
    public void SetDialogueUI()
    {

    }

    public void HandleTag(List<string> tags)
    {
        string speakerName = "";
        string spriteName = "";

        foreach(string tag in tags)
        {

        }
    }
}
