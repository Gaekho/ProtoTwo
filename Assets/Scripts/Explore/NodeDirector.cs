using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

// In NodeScene Root
public class NodeDirector : MonoBehaviour
{
    [SerializeField] private string currentNodeId;
    // Start is called before the first frame update
    void Start()
    {
        currentNodeId = RuntimeState.Instance.GetCurrentNodeId();
        Debug.Log($"{currentNodeId} / Visited={RuntimeState.Instance.GetNodeState(currentNodeId).Visited}");

        // Set Node Visited
        RuntimeState.Instance.GetNodeState(currentNodeId).VisitNode();

        var nodeState = RuntimeState.Instance.GetNodeState(currentNodeId);
        //Check Battle Encounter
        if(nodeState.HasEncounter)
        {
            Debug.Log("Battle Scene Start");
            //Enabled Interaction Triggers
            gameObject.GetComponentInChildren<Canvas>().gameObject.SetActive(false);
            StartCoroutine(SceneFlowManager.Instance.RequestBattleEncounter());
            
            //Skip other lines
            return;
        }

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

        //Check Story Trigger
        if (StoryProgressManager.Instance.ShouldStoryTrigger(currentNodeId))
        {
            Debug.Log("Should Story Trigger");
        }

        Debug.Log($"{currentNodeId} / Visited={nodeState.Visited} / Danger={nodeState.DangerLevel} / Encounter={nodeState.HasEncounter}");
    }

    
}
