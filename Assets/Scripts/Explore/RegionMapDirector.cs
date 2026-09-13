using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionMapDirector : MonoBehaviour
{
    [SerializeField] private string currentRegionId;

    [SerializeField] private List<MapNode> nodeList;
    // Start is called before the first frame update
    void Start()
    {
        foreach (MapNode node in nodeList)
        {
            //node.UpdateVisual(RuntimeState.Instance.GetNodeState(node.OwnNodeId));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
