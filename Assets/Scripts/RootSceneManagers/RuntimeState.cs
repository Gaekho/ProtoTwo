using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeState : MonoBehaviour
{
    #region Singleton
    private RuntimeState() { }
    public static RuntimeState Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    
    //Current Node
    [SerializeField] private string currentRegionId;    // Length 2 (Head)
    [SerializeField] private string currentLocalId;     // Length 2 (Rear)
    [SerializeField] private AreaType currentAreaType;  // Village || Dungeon

    //Party State
    private PartyState currentPartyState;
    private DeckData currentDeck;
    //HashSet<int> ownedCardIds;

    //Region State <RegionId, value>
    private Dictionary<string, int> suspicion;
    private Dictionary<string , int> affinity;
    
    // Quest State
    private Dictionary<int, bool> questFlags;

    //Node State
    private Dictionary<string, NodeState> nodeStates = new ();

    #region Public Methods

    // Node ID Get & Set
    public void SetCurrentNode(string fullNodeId)
    {
        // Split by region number & Node Id
        (string region, string local) = SplitNodeId(fullNodeId);
        currentRegionId = region;
        currentLocalId = local;
    }
    public string GetCurrentNodeId() => currentRegionId + currentLocalId;

    public string GetCurrentRegionId() => currentRegionId;
    public string GetCurrentLocalId() => currentLocalId;

    private (string region, string local) SplitNodeId(string fullNodeId)
    {
        if (fullNodeId.Length != 4) Debug.Log("Node Id should be length 4");
        string region = fullNodeId.Substring(0, 2);
        string local = fullNodeId.Substring(2, 2);
        return (region, local);
    }
    
    // Node State Get & Set
    //public NodeDangerLevel GetNodeDangerLevel(string nodeId)
    //{
    //    return nodeDangerLevels[nodeId];
    //}
    //public void SetNodeDangerousLevel(string nodeId, NodeDangerLevel danger)
    //{
    //    nodeDangerLevels[nodeId] = danger;
    //}
    //public bool HasActiveEncounter(string nodeId)
    //{
    //    return nodeEncounterActive[nodeId];
    //}
    //public void SetNodeEncounter(string nodeId, bool active)
    //{
    //    nodeEncounterActive[nodeId] = active;
    //}

    public NodeState GetNodeState(string nodeId)
    {
        if(!nodeStates.TryGetValue(nodeId, out var state))
        {
            state = new NodeState();
            nodeStates[nodeId] = state;
        }

        return state;
    }

    #endregion
}
