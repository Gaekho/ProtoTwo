using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proto2.Enums;
// In NodeScene Root
public class NodeDirector : MonoBehaviour
{
    [SerializeField] private string currentNodeId;
    [SerializeField] private List<int> nodeStoryEvents = new();
    [SerializeField] private TextAsset nodeInteractionInk;
    [SerializeField] private Canvas ownCanvas;

    public string CurrentNodeId => currentNodeId;
    // Start is called before the first frame update
    private void Start()
    {
        currentNodeId = RuntimeState.Instance.GetCurrentNodeId();
        
        var nodeState = RuntimeState.Instance.GetNodeState(currentNodeId);

        ownCanvas = GetComponentInChildren<Canvas>();
        Debug.Log($"{currentNodeId} / Visited={nodeState.Visited}");

        // Set Node Visited
        nodeState.VisitNode();
        nodeState.SetAccessable(NodeAccessable.CurrentNode);

        //Update Suspicious
        switch (nodeState.DangerLevel)
        {
            case NodeDangerLevel.Safe: 
               
                break;

            case NodeDangerLevel.Caution: 
                
                break;
            
            case NodeDangerLevel.Danger: 
                
                break;
        }

        CheckEntryCondition();
    }
    private void CheckEntryCondition()
    {
        var nodeState = RuntimeState.Instance.GetNodeState(currentNodeId);

        // Check Battle Encounter
        if (nodeState.Encounter) 
        { 
            Debug.Log("Battle Scene Start");
            //Enabled Interaction Triggers
            ownCanvas.gameObject.SetActive(false);
            SceneFlowManager.Instance.RequestBattleEncounter();

            GameEventsLibrary.OnBattleEnd += HandleBattleEnd;
            //Skip other lines
            return;
        }

        // Check Story Trigger
        foreach(int eventId in nodeStoryEvents)
        {
            if(StoryProgressLibrary.TryTriggerStoryEvent(eventId, out string path))
            {
                StartDialogue(path);
                break;
            }
        }
    }

    private void HandleBattleEnd() 
    {
        Debug.Log("HandleBattleEnd");
        var nodeState = RuntimeState.Instance.GetNodeState(currentNodeId);
        nodeState.SetEncounter(false);

        ownCanvas.gameObject.SetActive(true);

        GameEventsLibrary.OnBattleEnd -= HandleBattleEnd;
        
        CheckEntryCondition();
    }

    public void InteractionProcess(string interactionKey)
    {
        var nodeState = RuntimeState.Instance.GetNodeState(currentNodeId);
        nodeState.IncreaseInteraction(interactionKey);

        StartDialogue(InkPathProvider.GetNodeInteractionKnot(currentNodeId, interactionKey));
    }
    public void StartDialogue(string path)
    {
        DialogueManager.Instance.StartStory(path, gameObject.transform, nodeInteractionInk);
    }

}
