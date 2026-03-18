using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Proto2.Enums;

/// <summary>
/// 슬레이 더 스파이어식 "맵 골격" 생성기
/// 
/// 이 스크립트는 아래만 담당합니다.
/// 1. 층별 노드 생성
/// 2. 시작점 여러 개에서 위층까지 경로 생성
/// 3. 교차를 최소화하며 연결
/// 4. 연결되지 않은 노드 제거
/// 5. 최상층 위에 보스 노드 1개 추가
/// 
/// 주의:
/// - "몇 층에 어떤 노드 타입이 나온다" 같은 규칙은 여기서 처리하지 않습니다.
/// - 모든 일반 노드는 기본 프리팹으로 생성합니다.
/// - 노드 타입 배정은 이후 별도 시스템에서 처리하도록 분리합니다.
/// </summary>
public class MapGenerator : MonoBehaviour
{
    [Header("Level Data")]
    [SerializeField] private int floors = 10;          // 일반 층 수 (보스층 제외)
    [SerializeField] private int minRoom = 1;          // 층당 최소 노드 수
    [SerializeField] private int maxRoom = 4;          // 층당 최대 노드 수
    [SerializeField] private NodeBase[] nodes;         // 노드 프리팹 목록
    [SerializeField] private int numOfStartingNodes = 2;     // 시작 경로 개수 용도로 사용

    [Header("Map Data")]
    [SerializeField] private Transform buttonPivot;    // 노드가 생성될 부모
    [SerializeField] private float nodeGap = 30f;      // 같은 층 내 노드 간격
    [SerializeField] private float floorGap = 30f;     // 층 간 간격

    [Header("Generator Option")]
    [SerializeField] private bool generateOnStart = true;

    // 생성된 노드 관리용
    private readonly Dictionary<Vector2Int, NodeBase> nodeMap = new();
    private readonly List<NodeBase> generatedNodes = new();
    private readonly List<Edge> edges = new();

    private NodeBase bossNode;

    /// <summary>
    /// 선 연결 정보를 저장하는 간단한 구조체
    /// 교차 검사에 사용합니다.
    /// </summary>
    private struct Edge
    {
        public Vector2Int from;
        public Vector2Int to;

        public Edge(Vector2Int from, Vector2Int to)
        {
            this.from = from;
            this.to = to;
        }
    }

    private void Start()
    {
        if (generateOnStart)
        {
            GenerateMap();
        }
    }

    [ContextMenu("Generate Map")]
    public void GenerateMap()
    {
        ClearMap();
        ValidateSettings();

        // 1. 먼저 여러 개의 시작 경로를 만들어 전체 맵의 뼈대를 구성
        GeneratePaths();

        // 2. 경로와 전혀 연결되지 않은 노드 제거
        RemoveIsolatedNodes();

        // 3. 최상층 위에 보스 노드 생성 후 연결
        CreateBossNode();

        Debug.Log("Map Generate Complete");
    }

    [ContextMenu("Clear Map")]
    public void ClearMap()
    {
        foreach (Transform child in buttonPivot)
        {
            DestroyImmediate(child.gameObject);
        }

        nodeMap.Clear();
        generatedNodes.Clear();
        edges.Clear();
        bossNode = null;
    }

    /// <summary>
    /// 설정값 보정
    /// </summary>
    private void ValidateSettings()
    {
        floors = Mathf.Max(2, floors);
        minRoom = Mathf.Max(1, minRoom);
        maxRoom = Mathf.Max(minRoom, maxRoom);
        numOfStartingNodes = Mathf.Clamp(numOfStartingNodes, 1, maxRoom);
    }

    // ------------------------------------------------------------------
    // 1. 경로 생성
    // ------------------------------------------------------------------

    /// <summary>
    /// 시작점 여러 개를 뽑고,
    /// 각 시작점에서 맨 위층까지 한 줄 경로를 생성합니다.
    /// 이후 각 층의 최소 노드 수를 만족하도록 보강합니다.
    /// </summary>
    private void GeneratePaths()
    {
        List<int> startColumns = GetUniqueStartColumns(numOfStartingNodes);

        // 시작점마다 1개씩 위로 올라가는 경로 생성
        foreach (int startX in startColumns)
        {
            int currentX = startX;
            //GetOrCreateNode(new Vector2Int(currentX, 0));

            for (int y = 0; y < floors - 1; y++)
            {
                int nextX = PickNextColumn(currentX, y);

                Vector2Int from = new Vector2Int(currentX, y);
                Vector2Int to = new Vector2Int(nextX, y + 1);

                NodeBase fromNode = GetOrCreateNode(from);
                NodeBase toNode = GetOrCreateNode(to);

                ConnectNodes(fromNode, toNode);
                currentX = nextX;
            }
        }

        // 각 층이 최소/최대 방 수 범위에 들어오도록 노드를 보강
        //EnsureFloorRoomCounts();

        // 시작층에서 도달 불가능한 노드 제거
        RemoveUnreachableNodes();
    }

    /// <summary>
    /// 시작층에서 사용할 시작 x 좌표를 중복 없이 뽑습니다.
    /// </summary>
    private List<int> GetUniqueStartColumns(int count)
    {
        List<int> pool = Enumerable.Range(0, maxRoom)
            .OrderBy(_ => Random.value)
            .ToList();

        return pool.Take(count).ToList();
    }

    /// <summary>
    /// 현재 칸에서 다음 층으로 이동할 x 좌표를 선택합니다.
    /// 기본적으로 좌, 중앙, 우 중 하나로만 이동합니다.
    /// 가능하면 기존 선과 교차하지 않는 후보를 우선 선택합니다.
    /// </summary>
    private int PickNextColumn(int currentX, int currentY)
    {
        List<int> candidates = new();

        for (int dx = -1; dx <= 1; dx++)
        {
            int nextX = currentX + dx;
            if (nextX < 0 || nextX >= maxRoom)
                continue;

            Edge candidate = new Edge(
                new Vector2Int(currentX, currentY),
                new Vector2Int(nextX, currentY + 1)
            );

            if (!IsCrossing(candidate))
            {
                candidates.Add(nextX);
            }
        }

        // 교차를 피할 수 있는 후보가 없으면 그냥 인접 칸 허용
        if (candidates.Count == 0)
        {
            //for (int dx = -1; dx <= 1; dx++)
            //{
            //    int nextX = currentX + dx;
            //    if (nextX < 0 || nextX >= horizontalSlots)
            //        continue;

            //    candidates.Add(nextX);
            //}
            candidates.Add(currentX);
        }

        return candidates[Random.Range(0, candidates.Count)];
    }

    /// <summary>
    /// 같은 층에서 시작해서 다음 층으로 가는 두 선이 X자 형태로 교차하는지 검사합니다.
    /// </summary>
    private bool IsCrossing(Edge candidate)
    {
        foreach (Edge existing in edges)
        {
            // 같은 층에서 시작하는 간선끼리만 교차 검사
            if (existing.from.y != candidate.from.y)
                continue;

            bool crosses =
                (candidate.from.x < existing.from.x && candidate.to.x > existing.to.x) ||
                (candidate.from.x > existing.from.x && candidate.to.x < existing.to.x);

            if (crosses)
                return true;
        }

        return false;
    }

    /// <summary>
    /// 각 층의 노드 수가 minRoom ~ maxRoom 사이가 되도록 보강합니다.
    /// 
    /// 새 노드를 만들면 반드시 이전 층 또는 다음 층과 연결을 시도합니다.
    /// 그래야 고립 노드가 되지 않습니다.
    /// </summary>
    private void EnsureFloorRoomCounts()
    {
        for (int currentFloor = 0; currentFloor < floors; currentFloor++)
        {
            //int targetCount = Random.Range(minRoom, maxRoom + 1);
            //List<NodeBase> floorNodes = GetNodesAtFloor(currentFloor);

            //while (floorNodes.Count < targetCount)
            //{
            //    int x = Random.Range(0, horizontalSlots);
            //    Vector2Int pos = new Vector2Int(x, currentFloor);

            //    if (nodeMap.ContainsKey(pos))
            //    {
            //        floorNodes = GetNodesAtFloor(currentFloor);
            //        continue;
            //    }

            //    NodeBase newNode = GetOrCreateNode(pos);

            //    // 아래층과 연결
            //    if (currentFloor > 0)
            //    {
            //        TryConnectToClosestFloor(newNode, currentFloor - 1, connectFromOtherToThis: true);
            //    }

            //    // 위층과 연결
            //    if (currentFloor < floors - 1)
            //    {
            //        TryConnectToClosestFloor(newNode, currentFloor + 1, connectFromOtherToThis: false);
            //    }

            //    floorNodes = GetNodesAtFloor(currentFloor);
            //}

            int targetCount = Random.Range(minRoom, maxRoom + 1);
            int currentCount = GetNodesAtFloor(currentFloor).Count;

            while (currentCount < targetCount)
            {
                int x = Random.Range(0, maxRoom);
                Vector2Int pos = new Vector2Int(x, currentFloor);

                if (nodeMap.ContainsKey(pos))
                {
                    continue;
                }

                NodeBase newNode = GetOrCreateNode(pos);

                if (currentFloor > 0)
                {
                    TryConnectToClosestFloor(newNode, currentFloor - 1, true);
                }

                if (currentFloor < floors - 1)
                {
                    TryConnectToClosestFloor(newNode, currentFloor + 1, false);
                }

                currentCount++;
            }
        }
    }

    /// <summary>
    /// 지정한 층의 노드들 중, source와 x축으로 가장 가까운 노드와 연결합니다.
    /// </summary>
    private void TryConnectToClosestFloor(NodeBase source, int targetFloor, bool connectFromOtherToThis)
    {
        List<NodeBase> candidates = GetNodesAtFloor(targetFloor);
        if (candidates.Count == 0)
            return;

        NodeBase nearest = candidates
            .OrderBy(n => Mathf.Abs(n.GetPosition().x - source.GetPosition().x))
            .FirstOrDefault();

        if (nearest == null)
            return;

        if (connectFromOtherToThis)
        {
            ConnectNodes(nearest, source);
        }
        else
        {
            ConnectNodes(source, nearest);
        }
    }

    // ------------------------------------------------------------------
    // 2. 노드 생성 / 연결
    // ------------------------------------------------------------------

    /// <summary>
    /// grid 좌표에 해당하는 노드가 있으면 반환하고,
    /// 없으면 새로 생성합니다.
    /// </summary>
    private NodeBase GetOrCreateNode(Vector2Int gridPos)
    {
        if (nodeMap.TryGetValue(gridPos, out NodeBase existing))
        {
            return existing;
        }

        NodeBase prefab = GetDefaultNodePrefab();
        if (prefab == null)
        {
            Debug.LogError("기본 노드 프리팹이 없습니다.");
            return null;
        }

        NodeBase created = Instantiate(prefab, buttonPivot);
        created.name = $"Node_{gridPos.x}_{gridPos.y}";

        created.SetPosition(gridPos.x, gridPos.y);
        created.SetNodeIndex(generatedNodes.Count);

        RectTransform rect = created.GetComponent<RectTransform>();
        Vector2 localPos = GridToLocalPosition(gridPos.x, gridPos.y);

        if (rect != null)
            rect.anchoredPosition = localPos;
        else
            created.transform.localPosition = localPos;

        created.nextNodes ??= new List<NodeBase>();
        created.prevNodes ??= new List<NodeBase>();

        nodeMap.Add(gridPos, created);
        generatedNodes.Add(created);

        return created;
    }

    /// <summary>
    /// 두 노드를 단방향으로 연결합니다.
    /// from -> to
    /// </summary>
    private void ConnectNodes(NodeBase from, NodeBase to)
    {
        if (from == null || to == null)
            return;

        if (from.nextNodes.Contains(to))
            return;

        from.nextNodes.Add(to);
        to.prevNodes.Add(from);

        from.SetConnected();
        to.SetConnected();

        edges.Add(new Edge(
            new Vector2Int((int)from.GetPosition().x, (int)from.GetPosition().y),
            new Vector2Int((int)to.GetPosition().x, (int)to.GetPosition().y)
        ));
    }

    /// <summary>
    /// grid 좌표를 실제 로컬 좌표로 변환합니다.
    /// 가운데 정렬 형태로 배치합니다.
    /// </summary>
    private Vector2 GridToLocalPosition(int x, int y)
    {
        float width = (maxRoom - 1) * nodeGap;
        float startX = -width * 0.5f;

        float posX = startX + x * nodeGap;
        float posY = y * floorGap;

        return new Vector2(posX, posY);
    }

    // ------------------------------------------------------------------
    // 3. 정리 단계
    // ------------------------------------------------------------------

    /// <summary>
    /// 완전히 고립된 노드 제거
    /// prev도 없고 next도 없는 노드 제거
    /// </summary>
    private void RemoveIsolatedNodes()
    {
        List<NodeBase> removeTargets = generatedNodes
            .Where(n => n.prevNodes.Count == 0 && n.nextNodes.Count == 0)
            .ToList();

        foreach (NodeBase node in removeTargets)
        {
            RemoveNode(node);
        }
    }

    /// <summary>
    /// 시작층에서 실제로 도달 가능한 노드만 남깁니다.
    /// </summary>
    private void RemoveUnreachableNodes()
    {
        List<NodeBase> startNodes = GetNodesAtFloor(0);
        HashSet<NodeBase> reachable = new();
        Queue<NodeBase> queue = new();

        foreach (NodeBase node in startNodes)
        {
            reachable.Add(node);
            queue.Enqueue(node);
        }

        while (queue.Count > 0)
        {
            NodeBase current = queue.Dequeue();

            foreach (NodeBase next in current.nextNodes)
            {
                if (next == null)
                    continue;

                if (reachable.Add(next))
                {
                    queue.Enqueue(next);
                }
            }
        }

        List<NodeBase> removeTargets = generatedNodes
            .Where(n => !reachable.Contains(n))
            .ToList();

        foreach (NodeBase node in removeTargets)
        {
            RemoveNode(node);
        }
    }

    /// <summary>
    /// 노드를 맵에서 제거합니다.
    /// 연결 정보도 같이 정리합니다.
    /// </summary>
    private void RemoveNode(NodeBase node)
    {
        foreach (NodeBase prev in node.prevNodes)
        {
            if (prev != null)
                prev.nextNodes.Remove(node);
        }

        foreach (NodeBase next in node.nextNodes)
        {
            if (next != null)
                next.prevNodes.Remove(node);
        }

        Vector2Int key = new Vector2Int((int)node.GetPosition().x, (int)node.GetPosition().y);

        nodeMap.Remove(key);
        generatedNodes.Remove(node);

        DestroyImmediate(node.gameObject);
    }

    // ------------------------------------------------------------------
    // 4. 보스 노드 생성
    // ------------------------------------------------------------------

    /// <summary>
    /// 마지막 일반 층 위에 보스 노드를 하나 생성하고,
    /// 최상층의 모든 노드를 보스 노드에 연결합니다.
    /// </summary>
    private void CreateBossNode()
    {
        List<NodeBase> topFloorNodes = GetNodesAtFloor(floors - 1);
        if (topFloorNodes.Count == 0)
            return;

        NodeBase bossPrefab = GetBossNodePrefab();
        if (bossPrefab == null)
        {
            Debug.LogWarning("Boss 노드 프리팹이 없습니다.");
            return;
        }

        bossNode = Instantiate(bossPrefab, buttonPivot);
        bossNode.name = "Boss_Node";

        // 보스 노드는 floors 번째 줄에 위치 (일반 층보다 한 칸 위)
        bossNode.SetPosition(maxRoom / 2, floors);
        bossNode.SetNodeIndex(generatedNodes.Count);

        RectTransform rect = bossNode.GetComponent<RectTransform>();
        Vector2 bossPos = new Vector2(0f, floors * floorGap);

        if (rect != null)
            rect.anchoredPosition = bossPos;
        else
            bossNode.transform.localPosition = bossPos;

        bossNode.nextNodes ??= new List<NodeBase>();
        bossNode.prevNodes ??= new List<NodeBase>();

        generatedNodes.Add(bossNode);

        foreach (NodeBase top in topFloorNodes)
        {
            ConnectNodes(top, bossNode);
        }
    }

    // ------------------------------------------------------------------
    // 5. 헬퍼
    // ------------------------------------------------------------------

    /// <summary>
    /// 특정 층의 노드 목록 반환
    /// </summary>
    private List<NodeBase> GetNodesAtFloor(int floor)
    {
        return generatedNodes
            .Where(n => Mathf.RoundToInt(n.GetPosition().y) == floor)
            .OrderBy(n => n.GetPosition().x)
            .ToList();
    }

    /// <summary>
    /// 일반 노드용 기본 프리팹 반환
    /// 
    /// 현재는 nodes[0]을 기본 노드 프리팹으로 사용합니다.
    /// 추후 필요하면 별도 필드로 분리하는 것이 더 안전합니다.
    /// </summary>
    private NodeBase GetDefaultNodePrefab()
    {
        if (nodes == null || nodes.Length == 0)
            return null;

        return nodes[0];
    }

    /// <summary>
    /// 보스 노드 프리팹 반환
    /// 
    /// 현재는 nodes 배열 안에서 NodeType.Boss를 가진 프리팹을 찾습니다.
    /// 없으면 null 반환.
    /// </summary>
    private NodeBase GetBossNodePrefab()
    {
        if (nodes == null)
            return null;

        foreach (NodeBase node in nodes)
        {
            if (node != null && node.GetNodeType() == NodeType.BossBattle)
                return node;
        }

        return null;
    }
}