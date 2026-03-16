using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventGenerator : MonoBehaviour
{
    [Header("Level Data")]
    //�� ����
    [SerializeField] private int floors = 10;
    //���� �ּ� �� ����
    [SerializeField] private int minRoom = 1;
    //���� �ִ� �� ����
    [SerializeField] private int maxRoom = 4;
    //�ʼ� ���
    [SerializeField] private EssentialNode essential;
    //��� ����
    [SerializeField] private NodeBase[] nodes;
    //Ư�� ��� ��(����Ʈ, ���� ��)
    [SerializeField] private int specialNodes = 2;

    [Header("Map Data")]
    //��带 ��ġ�� ��ǥ ������
    [SerializeField] private Transform buttonPivot;
    //���� �� ��尣�� ����
    [SerializeField] private float nodeGap = 10.0f;
    //���� ����
    [SerializeField] private float floorGap = 10.0f;

    //���� ��� ����
    private int numOfNextFloorNode = 0;
    //�Ʒ��� ����
    private List<NodeBase> lastFloorNode = new List<NodeBase>();
    //���� �� ����. �˰����� �ӽ� �����
    private List<NodeBase> currentNodes = new List<NodeBase>();

    //�ʿ� ��ġ�� ���
    private List<NodeBase>[] nodeTiles;
    private List<Vector2Int>[] nodeEdges;

    private void Awake()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        Vector3 basePosition = buttonPivot.position;
        //�ʼ� ��� ���� ���� ���߱� ���� 1���� ����
        for (int i = 1; i <= floors; i++)
        {
            GenerateFloor(i, buttonPivot,basePosition);
            basePosition.y += floorGap;
        }

        //InitializeMap();
        //GeneratePaths();
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

                // ������ ������ 3�� ��� (x-1, x, x+1) Ž��
                for (int dx = -1; dx <= 1; dx++)
                {
                    int nextX = nodeX + dx;
                    if (nextX >= 0 && nextX < floors)
                    {
                        // ��Ģ: ���(��)�� ���� ������ �� ����
                        if (!IsCrossing(f, nodeX, nextX))
                        {
                            validNextX.Add(nextX);
                        }
                    }
                }

                if (validNextX.Count == 0)
                    validNextX.Add(nodeX); // ���� ���� ������ ������ġ (����)

                int chosenX = validNextX[Random.Range(0, validNextX.Count)];
                NodeBase nextNode = nodeTiles[f + 1][chosenX];

                // ��� ����
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

    

    // --- ������ �ð�ȭ�� �ڵ� ---
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

                // ���ἱ �׸���
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
        numOfNextFloorNode = Random.Range(minRoom, maxRoom);

        if (currentFloor == essential.GetFloor())
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

        //ù�� ���� Ȱ��ȭ
        if (currentFloor == 1)
        {
            foreach (NodeBase node in currentNodes)
            {
                node.SetActivate();
            }
        }

        lastFloorNode.Clear();
        lastFloorNode = currentNodes;
        currentNodes.Clear();
    }

    private void GenerateNode(NodeType nodeType, int nodeAmount, Transform pivot, Vector3 basePosition)
    {
        for (int i = 0; i < nodeAmount; i++)
        {
            NodeBase makingNode = MatchNode(nodeType);
            if (makingNode != null)
            {
                NodeBase newNode = Instantiate(makingNode, pivot);
                newNode.transform.position = basePosition;
                currentNodes.Add(newNode);
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