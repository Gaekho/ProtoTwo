using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// In NodeScene Root
public class NodeSceneCotroller : MonoBehaviour
{
    [SerializeField] private string currentNode;
    // Start is called before the first frame update
    void Start()
    {
        currentNode = RuntimeState.Instance.GetCurrentNodeId();

        //Check Battle Encounter
        if(RuntimeState.Instance.HasActiveEncounter(currentNode))
        {
            Debug.Log("Battle Scene Start");
            //Enabled Interaction Triggers
            gameObject.GetComponentInChildren<Canvas>().gameObject.SetActive(false);
            StartCoroutine(SceneFlowManager.Instance.RequestBattleEncounter());
            
            //Skip other lines
            return;
        }

        //Update Suspicious
        switch (RuntimeState.Instance.GetNodeDangerLevel(currentNode))
        {
            case Proto2.Enums.NodeDangerLevel.Safe: 
               
                break;

            case Proto2.Enums.NodeDangerLevel.Caution: 
                
                break;
            
            case Proto2.Enums.NodeDangerLevel.Danger: 
                
                break;
        }

        //Check Story Trigger
        if (StoryProgressManager.Instance.ShouldTrigger(currentNode))
        {
            Debug.Log("Should Story Trigger");
        }
    }

    
}
