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
        if (Instance == null)   Instance = this;

        else    Destroy(gameObject);
        
    }
    #endregion

    [Header("Dialogue UI Prefab")]
    [SerializeField] private GameObject dialogueCanvas;

    [Header("Ink Story Library")]
    [SerializeField] private TextAsset storyInk;
    [SerializeField] private TextAsset questInk; 

    private Story currentStory;

    private TextAsset ResolveInkFile(string path, TextAsset sceneOverrideInk)
    {
        if (path.StartsWith("node_")) return sceneOverrideInk;
        if (path.StartsWith("story_")) return storyInk;
        if(path.StartsWith("quest_")) return questInk;

        Debug.Log($"Wrong Ink Path : {path}");
        return null;
    }
    //public void StartStory(TextAsset inkJson, string knotName, Transform parentTransform, Action onComplete = null)
    //{
    //    currentStory = new Story(inkJson.text);

    //    //Debug.Log($"current Story : {currentStory }");

    //    if (!string.IsNullOrEmpty(knotName))
    //    {
    //        currentStory.ChoosePathString(knotName);
    //    }

    //    GameObject activeCanvasInstance = Instantiate(dialogueCanvas, parentTransform);

    //    DialogueUIController uiController = activeCanvasInstance.GetComponentInChildren<DialogueUIController>();

    //    uiController.Initialize(activeCanvasInstance, onComplete);
    //    //Debug.Log($"uiController : {uiController.gameObject.name}");
    //    //AdvanceStory();
    //}

    public void StartStory(string path, Transform parentTransform, TextAsset sceneOverrideInk = null, Action onComplete = null)
    {
        TextAsset inkJson = ResolveInkFile(path, sceneOverrideInk);
        if(inkJson == null)
        {
            Debug.Log($"StartStroy Aborted : Ink File NOT Resolved ({path})");
            return;
        }

        currentStory = new Story(inkJson.text);

        if (!string.IsNullOrEmpty(path))
        {
            currentStory.ChoosePathString(path);
        }

        GameObject activeCanvasInstance = Instantiate(dialogueCanvas, parentTransform);
        DialogueUIController uiController = activeCanvasInstance.GetComponentInChildren<DialogueUIController>();
        uiController.Initialize(activeCanvasInstance, onComplete);
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
