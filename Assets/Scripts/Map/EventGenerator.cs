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
    [SerializeField] private Transform buttonPivot;
    //같은 층 노드간의 간격
    [SerializeField] private float nodeGap = 10.0f;
    //층간 간격
    [SerializeField] private float floorGap = 10.0f;

    //윗층 노드 갯수
    private int numOfNextFloorNode = 0;
    //아랫층 노드들
    private List<NodeBase> lastFloorNode = new List<NodeBase>();
    //현재 층 노드들. 알고리즘상 임시 저장용
    private List<NodeBase> currentNodes = new List<NodeBase>();

    //맵에 설치된 노드
    private List<NodeBase>[] nodeTiles;
    private List<Vector2Int>[] nodeEdges;

    private void Awake()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        //Vector3 basePosition = buttonTestTransform.position;
        ////필수 노드 지정 층과 맞추기 위해 1부터 시작
        //for (int i = 1; i <= floors; i++)
        //{
        //    GenerateFloor(i, buttonTestTransform,basePosition);
        //    basePosition.y += floorGap;
        //}

        InitializeMap();
        GeneratePaths();
    }

    private void InitializeMap()
    {
        nodeTiles = new List<NodeBase>[floors];
        nodeEdges = new List<Vector2Int>[floors - 1];

        for(int i = 0; i < floors; i++)
        {
            nodeTiles[i] = new List<NodeBase>();

            if((i + 1) == essential.GetFloor())
            {
                for (int j = 0; j < maxRoom; j++)
                {
                    nodeTiles[i].Add(MatchNode(essential.GetNodeType()));
                    nodeTiles[i][j].SetPosition(j, i);
                }
            }
            else
            {
                for(int j = 0;j < maxRoom; j++)
                {
                    int randomRoon = Random.Range(0, nodes.Length - specialNodes);
                    nodeTiles[i].Add(nodes[randomRoon]);
                    nodeTiles[i][j].SetPosition(j, i);
                }
            }
        }
    }

    private void GeneratePaths()
    {
        List<NodeBase> firstFloorStarts = new List<NodeBase>();

        for (int p = 0; p < maxRoom; p++)
        {
            int currentX = Random.Range(0, maxRoom);

            NodeBase currentNode = nodeTiles[0][currentX];
            currentNode.SetConnected();
            if (p == 0 || p == 1) firstFloorStarts.Add(currentNode);

            for (int f = 0; f < floors - 1; f++)
            {
                List<int> validNextX = new List<int>();
                int nodeX = (int)currentNode.GetPosition().x;

                // 위층의 인접한 3개 노드 (x-1, x, x+1) 탐색
                for (int dx = -1; dx <= 1; dx++)
                {
                    int nextX = nodeX + dx;
                    if (nextX >= 0 && nextX < floors)
                    {
                        // 규칙: 경로(선)는 서로 교차할 수 없음
                        if (!IsCrossing(f, nodeX, nextX))
                        {
                            validNextX.Add(nextX);
                        }
                    }
                }

                if (validNextX.Count == 0)
                    validNextX.Add(nodeX); // 교착 상태 방지용 안전장치 (직진)

                int chosenX = validNextX[Random.Range(0, validNextX.Count)];
                NodeBase nextNode = nodeTiles[f + 1][chosenX];

                // 노드 연결
                if (!currentNode.nextNodes.Contains(nextNode))
                {
                    currentNode.nextNodes.Add(nextNode);
                    nextNode.prevNodes.Add(currentNode);
                    nodeEdges[f].Add(new Vector2Int(nodeX, chosenX));
                }

                nextNode.SetConnected();
                currentNode = nextNode;
            }
        }
    }

    private bool IsCrossing(int floor, int fromX, int toX)
    {
        if (nodeEdges.Length != 0)
        {
            foreach (var edge in nodeEdges[floor])
            {
                int eFrom = edge.x;
                int eTo = edge.y;

                if (fromX < eFrom && toX > eTo) return true;
                if (fromX > eFrom && toX < eTo) return true;
            }
        }
        return false;
    }

    private void AssignNodeTypes()
    {
        for (int f = 0; f < floors; f++)
        {
            foreach (var node in nodeTiles[f])
            {
                if (!node.IsConnected()) {  continue; }

                NodeBase newNode = Instantiate(node, buttonPivot);
                Vector3 basePosition = buttonPivot.position;
                basePosition += new Vector3((newNode.GetPosition().x * nodeGap), (newNode.GetPosition().y * floorGap), 0);
                newNode.transform.position = basePosition;
            }
        }
    }

    

    // --- 에디터 시각화용 코드 ---
    private void OnDrawGizmos()
    {
        if (nodeTiles == null) return;

        float spacingX = 1.5f;
        float spacingY = 2.0f;

        for (int f = 0; f < floors; f++)
        {
            foreach (var node in nodeTiles[f])
            {
                if (!node.IsConnected()) continue;

                Vector3 pos = new Vector3(node.GetPosition().x * spacingX - (floors * spacingX / 2f), f * spacingY, 0);

                // 연결선 그리기
                Gizmos.color = Color.white;
                foreach (var next in node.nextNodes)
                {
                    Vector3 nextPos = new Vector3(next.GetPosition().x * spacingX - (floors * spacingX / 2f), next.GetPosition().y * spacingY, 0);
                    Gizmos.DrawLine(pos, nextPos);
                }
            }
        }
    }

    private void GenerateFloor(int currentFloor, Transform pivot, Vector3 basePosition)
    {
        //numOfNextFloorNode = Random.Range(minRoom, maxRoom);

        //if(currentFloor == essential.GetFloor())
        //{
        //    GenerateNode(essential.GetNodeType(), numOfNextFloorNode, pivot, basePosition);
        //}
        //else
        //{
        //    int nodeTypes = nodes.Length - specialNodes;
        //    int randomNode = Random.Range(0, nodeTypes);
        //    GenerateNode((NodeType)randomNode, numOfNextFloorNode, pivot, basePosition);
        //}

        //foreach (NodeBase node in lastFloorNode)
        //{
        //    ConnectNode(node);
        //}

        ////첫층 노드들 활성화
        //if(currentFloor == 1)
        //{
        //    foreach(NodeBase node in currentNodes)
        //    {
        //        node.SetActivate();
        //    }
        //}

        //lastFloorNode.Clear();
        //lastFloorNode = currentNodes;
        //currentNodes.Clear();
    }

    private void GenerateNode(NodeType nodeType, int nodeAmount, Transform pivot, Vector3 basePosition)
    {
        //for (int i = 0; i < nodeAmount; i++)
        //{
        //    NodeBase makingNode = MatchNode(nodeType);
        //    if(makingNode != null)
        //    {
        //        NodeBase newNode = Instantiate(makingNode, pivot);
        //        newNode.transform.position = basePosition;
        //        currentNodes.Add(newNode);
        //    }
        //    basePosition += new Vector3(nodeGap, 0, 0);
        //}
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
        //int numOfNodeToConnect = Random.Range(1, numOfNextFloorNode);
        //int index = node.GetNodeIndex();
        //int changeIndex = 0;
        //bool bIsNeighobrChecked = false;

        //for(int i = 0; i < numOfNodeToConnect; i++)
        //{
        //    if (bIsNeighobrChecked)
        //    {
        //        changeIndex++;
        //    }
        //    else
        //    {
        //        changeIndex *= -1;
        //    }

        //    int indexToConnect = index + changeIndex;

        //    if (indexToConnect >= 0)
        //    {
        //        node.ConnectNode(currentNodes[indexToConnect]);
        //        bIsNeighobrChecked = !bIsNeighobrChecked;
        //    }
        //    else
        //    {
        //        indexToConnect *= -1;
        //        node.ConnectNode(currentNodes[indexToConnect]);
        //        bIsNeighobrChecked = true;
        //    }
        //    i++;
        //}
    }
}