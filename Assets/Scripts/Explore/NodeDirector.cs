using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


        //Update Suspicious
        switch (nodeState.DangerLevel)
        {
            case Proto2.Enums.NodeDangerLevel.Safe: 
               
                break;

            case Proto2.Enums.NodeDangerLevel.Caution: 
                
                break;
            
            case Proto2.Enums.NodeDangerLevel.Danger: 
                
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

            GameEvents.OnBattleEnd += HandleBattleEnd;
            //Skip other lines
            return;
        }

        // Check Story Trigger
        foreach(int eventId in nodeStoryEvents)
        {
            if(StoryProgressManager.Instance.TryTriggerStoryEvent(eventId, out string path))
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

        GameEvents.OnBattleEnd -= HandleBattleEnd;
        
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
        TextAsset inkFile = ResolveInkFiles(path);
        DialogueManager.Instance.StartStory(inkFile, path, gameObject.transform);
    }

    private TextAsset ResolveInkFiles(string path)
    {
        if (path.StartsWith("node_")) return nodeInteractionInk;
        if (path.StartsWith("story_")) return InkLibraryManager.Instance.StoryInk;
        if (path.StartsWith("quest_")) return InkLibraryManager.Instance.QuestInk;

        Debug.Log($"Wrong Ink Path : {path}");
        return null;
    }
}
