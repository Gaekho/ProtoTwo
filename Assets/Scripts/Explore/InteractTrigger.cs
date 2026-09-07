using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTrigger : MonoBehaviour
{
    [SerializeField] private TextAsset inkJson;
    [SerializeField] private string interactionKey;

    [SerializeField] private Transform sceneRoot;

    public void TriggerDialogue()
    {
        NodeDirector director = gameObject.GetComponentInParent<NodeDirector>();
        RuntimeState.Instance.GetNodeState(director.CurrentNodeId).IncreaseInteraction(interactionKey);
        Debug.Log(RuntimeState.Instance.GetNodeState(director.CurrentNodeId).GetInteractionCount(interactionKey));
        director.StartDialogue(InkPathProvider.GetNodeInteractionKnot(director.CurrentNodeId, interactionKey));
        //DialogueManager.Instance.StartStory(inkJson, startKnotName, sceneRoot);
    }
}
