using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("Generator")]
    [SerializeField] private MapGenerator mapGenerator;

    //플레이어가 있는 위치
    public Vector2Int playerPosition = new Vector2Int(-1, -1);
    //맵에 배치된 노드들
    private Dictionary<Vector2Int, NodeBase> nodeMap = new();
    //한 층당 최대 노드 갯수
    private int maxNode = 4;

    private void Start()
    {
        mapGenerator.GenerateMap();

        nodeMap = mapGenerator.GetNodes();
        maxNode = mapGenerator.GetMaxRoom();

        ActivateFirstFloor();
    }

    private void ActivateFirstFloor()
    {
        for(int i = 0; i < maxNode;  i++)
        {
            Vector2Int nodePosition = new Vector2Int(i, 0);
            NodeBase node = nodeMap[nodePosition];
            if (node != null)
            {
                ActivateNodes(node);
            }
        }
    }

    private void ActivateNodes(NodeBase nodeToActivate)
    {
        nodeToActivate.SetActivate(true);
    }
}
