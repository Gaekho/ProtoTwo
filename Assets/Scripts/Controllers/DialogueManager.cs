using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Dialogue UI Prefab")]
    [SerializeField] private GameObject dialogueCanvas;

    private GameObject activeCanvasInstance;
    private DialogueUIController uiController;
    private Story currentStory;
    private Action onDialogueCompleteCallback;
    
    private bool dialoguePlaying = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool DialoguePlaying()
    {
        return dialoguePlaying;
    }
    public void StartStory(TextAsset inkJson, string knotName, Transform parentTransform, Action onComplete = null)
    {
        dialoguePlaying = true;
        onDialogueCompleteCallback = onComplete;
        currentStory = new Story(inkJson.text);

        Debug.Log($"current Story : {currentStory}");

        if (!string.IsNullOrEmpty(knotName))
        {
            currentStory.ChoosePathString(knotName);
        }

        activeCanvasInstance = Instantiate(dialogueCanvas, parentTransform);

        uiController = activeCanvasInstance.GetComponentInChildren<DialogueUIController>();

        Debug.Log($"uiController : {uiController.gameObject.name}");
        AdvanceStroy();
    }

    public void AdvanceStroy()
    {
        if (currentStory.canContinue)
        {
            string nextLine = currentStory.Continue();
            List<String> currentTags = currentStory.currentTags;

            uiController.SetDialogueUI(nextLine, currentStory.currentChoices, currentTags);
            Debug.Log("Story Advanced!");
        }
        else
        {
            EndStory();
        }
    }

    public void SelectChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        AdvanceStroy();
    }
    public void EndStory()
    {
        if (activeCanvasInstance != null)
        {
            Destroy(activeCanvasInstance);
            activeCanvasInstance = null;
            uiController = null;
        }
        dialoguePlaying = false;
    }

}
