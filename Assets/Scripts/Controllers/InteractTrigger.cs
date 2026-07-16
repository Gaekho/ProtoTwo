using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTrigger : MonoBehaviour
{
    [SerializeField] private TextAsset inkJson;
    [SerializeField] private string startKnotName;

    [SerializeField] private Transform sceneRoot;

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartStory(inkJson, startKnotName, sceneRoot);
    }
}
