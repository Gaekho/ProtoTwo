using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [Header("Deck")]
    [SerializeField] private DeckData playerDeck = null;

    private EncounterBase encounterData = null;

    //Setter
    public void SetEncounterData(EncounterBase inEncounter) { encounterData = inEncounter; }
    public void SetDeck(DeckData inDeck) { playerDeck = inDeck; }

    //Getter
    public DeckData GetPlayerDeck() { return playerDeck; }
    public EncounterBase GetEncounterData() { return encounterData; }

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
                Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
