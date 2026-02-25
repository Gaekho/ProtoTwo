using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventGenerator : MonoBehaviour
{
    [Header("Level Data")]
    //รั รþผ๖
    [SerializeField] private int floors = 10;
    [SerializeField] private int minRoom = 1;
    [SerializeField] private int maxRoom = 4;
    [SerializeField] private EssentialNode essential;
    [SerializeField] private NodeBase[] nodes;
    [SerializeField] private Transform buttonTestTransform;

    public void GenerateMap()
    {

    }

    private void GenerateFloor(int currentFloor)
    {
        int nodeAmount = Random.Range(minRoom, maxRoom);

        if(currentFloor == essential.GetFloor())
        {
            GenerateNode(essential.GetNodeType(), nodeAmount);
        }
        else
        {
            int randomNode = Random.Range(0, 3);

        }
    }

    private void GenerateNode(NodeType nodeType, int nodeAmount)
    {
        for (int i = 0; i < nodeAmount; i++)
        {
            NodeBase makingNode = MatchNode(nodeType);
            if(makingNode != null)
            {
                makingNode = 
            }
            buttonTestTransform.position += new Vector3(10, 0, 0);
        }
    }

    private NodeBase MatchNode(NodeType nodeType)
    {
        for(int i  = 0; i < nodes.Length; i++)
        {
            if(nodeType == nodes[i].GetNodeType())
            {
                return nodes[i];
            }
        }
        return null;
    }
}
