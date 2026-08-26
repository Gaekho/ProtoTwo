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
    string currentNodeId;
    string currentRegionId;

    //Party State
    private CurrentPartyState currentPartyState;
    public DeckData currentDeck;
    //HashSet<int> ownedCardIds;

    //Region State
    private Dictionary<string, int> suspicion;
    private Dictionary<string , int> affinity;
    
    // Quest State
    public Dictionary<int, bool> questFlags;

}
