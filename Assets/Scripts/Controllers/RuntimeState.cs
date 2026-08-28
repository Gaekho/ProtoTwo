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
    [SerializeField] private string currentNodeId;
    [SerializeField] private string currentRegionId;

    //Party State
    private CurrentPartyState currentPartyState;
    public DeckData currentDeck;
    //HashSet<int> ownedCardIds;

    //Region State <RegionId, value>
    private Dictionary<string, int> suspicion;
    private Dictionary<string , int> affinity;
    
    // Quest State
    public Dictionary<int, bool> questFlags;

    public void SetCurrentNode(string fullNodeId)
    {
        // Split by region number & Node Id
        //currentNodeId = nodeId;
        (string region, string node) = SplitNodeId(fullNodeId);
        currentRegionId = region;
        currentNodeId = node;
    }
    public string GetCurrentNodeId()
    {
        return (currentRegionId + currentNodeId);
    }

    public string GetCurrentRegionId()
    {
        return currentRegionId;
    }

    private (string region, string node) SplitNodeId(string fullNodeId)
    {
        if (fullNodeId.Length != 4) Debug.Log("Node Id should be length 4");
        string region = fullNodeId.Substring(0, 2);
        string node = fullNodeId.Substring(2, 2);
        return (region, node);
    }

}
