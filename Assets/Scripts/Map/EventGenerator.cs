using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

public class EventGenerator : MonoBehaviour
{
    [Header("Level Data")]
    //총 층수
    [SerializeField] private int floors = 10;
    //층당 최소 방 갯수
    [SerializeField] private int minRoom = 1;
    //층당 최대 방 갯수
    [SerializeField] private int maxRoom = 4;
    //필수 노드
    [SerializeField] private EssentialNode essential;
    //노드 정보
    [SerializeField] private NodeBase[] nodes;
    //특수 노드 수(엘리트, 보스 등)
    [SerializeField] private int specialNodes = 2;

    [Header("Map Data")]
    //노드를 배치할 좌표 기준점
    [SerializeField] private Transform buttonTestTransform;
    //같은 층 노드간의 간격
    [SerializeField] private float nodeGap = 10.0f;
    //층간 간격
    [SerializeField] private float floorGap = 10.0f;

    //윗층 노드 갯수
    private int numOfNextFloorNode = 0;
    //아랫층 노드들
    private List<NodeBase> lastFloorNode = new List<NodeBase>();
    //현재 층 노드들. 알고리즘상 임시 저장용
    private List<NodeBase> currenNode = new List<NodeBase>();

    private void Awake()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        Vector3 basePosition = buttonTestTransform.position;
        //필수 노드 지정 층과 맞추기 위해 1부터 시작
        for (int i = 1; i <= floors; i++)
        {
            GenerateFloor(i, buttonTestTransform,basePosition);
            buttonTestTransform.position += new Vector3(0, floorGap, 0);
        }
    }

    private void GenerateFloor(int currentFloor, Transform pivot, Vector3 basePosition)
    {
        numOfNextFloorNode = Random.Range(minRoom, maxRoom);

        if(currentFloor == essential.GetFloor())
        {
            GenerateNode(essential.GetNodeType(), numOfNextFloorNode, pivot, basePosition);
        }
        else
        {
            int nodeTypes = nodes.Length - specialNodes;
            int randomNode = Random.Range(0, nodeTypes);
            GenerateNode((NodeType)randomNode, numOfNextFloorNode, pivot, basePosition);
        }

        foreach (NodeBase node in lastFloorNode)
        {
            ConnectNode(node);
        }

        lastFloorNode.Clear();
        lastFloorNode = currenNode;
    }

    private void GenerateNode(NodeType nodeType, int nodeAmount, Transform pivot, Vector3 basePosition)
    {
        for (int i = 0; i < nodeAmount; i++)
        {
            NodeBase makingNode = MatchNode(nodeType);
            if(makingNode != null)
            {
                NodeBase newNode = Instantiate(makingNode, pivot);
                newNode.transform.position = basePosition;
                currenNode.Add(newNode);
            }
            basePosition += new Vector3(nodeGap, 0, 0);
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

    private void ConnectNode(NodeBase node)
    {
        int numOfNodeToConnect = Random.Range(1, numOfNextFloorNode);
        int index = node.GetNodeIndex();
        int changeIndex = 0;
        bool bIsNeighobrChecked = false;

        for(int i = 0; i < numOfNodeToConnect; i++)
        {
            if (bIsNeighobrChecked)
            {
                changeIndex++;
            }
            else
            {
                changeIndex *= -1;
            }

            int indexToConnect = index + changeIndex;

            if (indexToConnect >= 0)
            {
                node.ConnectNode(currenNode[indexToConnect]);
                bIsNeighobrChecked = !bIsNeighobrChecked;
            }
            else
            {
                indexToConnect *= -1;
                node.ConnectNode(currenNode[indexToConnect]);
                bIsNeighobrChecked = true;
            }
            i++;
        }
    }
}
