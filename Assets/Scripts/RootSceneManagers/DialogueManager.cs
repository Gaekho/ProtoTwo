using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System;

public class DialogueManager : MonoBehaviour
{
    #region Singleton
    public static DialogueManager Instance { get; private set; }
    private DialogueManager() { }
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
    #endregion

    [Header("Dialogue UI Prefab")]
    [SerializeField] private GameObject dialogueCanvas;

    private Story currentStory;

    public void StartStory(TextAsset inkJson, string knotName, Transform parentTransform, Action onComplete = null)
    {
        currentStory = new Story(inkJson.text);

        //Debug.Log($"current Story : {currentStory }");

        if (!string.IsNullOrEmpty(knotName))
        {
            currentStory.ChoosePathString(knotName);
        }

        GameObject activeCanvasInstance = Instantiate(dialogueCanvas, parentTransform);

        DialogueUIController uiController = activeCanvasInstance.GetComponentInChildren<DialogueUIController>();

        uiController.Initialize(activeCanvasInstance, onComplete);
        //Debug.Log($"uiController : {uiController.gameObject.name}");
        //AdvanceStory();
    }

    public bool AdvanceStory(out string nextLine, out List<Choice> choices, out List<string> tags)
    {
        if (currentStory.canContinue)
        {
            nextLine = currentStory.Continue();
            choices = currentStory.currentChoices;
            tags = currentStory.currentTags;
            return true;
        }

        nextLine = null;
        choices = null;
        tags = null;
        return false;
    }

    public void SelectChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
    }

}
