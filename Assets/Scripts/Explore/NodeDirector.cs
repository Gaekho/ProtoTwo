using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
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
        ownCanvas = GetComponentInChildren<Canvas>();
        Debug.Log($"{currentNodeId} / Visited={RuntimeState.Instance.GetNodeState(currentNodeId).Visited}");

        // Set Node Visited
        RuntimeState.Instance.GetNodeState(currentNodeId).VisitNode();

        var nodeState = RuntimeState.Instance.GetNodeState(currentNodeId);
        ////Check Battle Encounter
        //if(nodeState.HasEncounter)
        //{
        //    Debug.Log("Battle Scene Start");
        //    //Enabled Interaction Triggers
        //    gameObject.GetComponentInChildren<Canvas>().gameObject.SetActive(false);
        //    StartCoroutine(SceneFlowManager.Instance.RequestBattleEncounter());
            
        //    //Skip other lines
        //    return;
        //}

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
        if (nodeState.HasEncounter) 
        { 
            Debug.Log("Battle Scene Start");
            //Enabled Interaction Triggers
            ownCanvas.gameObject.SetActive(false);
            StartCoroutine(SceneFlowManager.Instance.RequestBattleEncounter());

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
