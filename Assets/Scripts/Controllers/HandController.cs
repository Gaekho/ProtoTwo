using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    #region Singlton
    private HandController() { }
    public static HandController Instance { get; private set; }
    #endregion

    #region Field
    [Header("Card Prefab")]
    [SerializeField] private GameObject basicCard;

    [Header("Hand Position")]
    [SerializeField] private Transform spawnParent;
    [SerializeField] private Vector3 startPoint;
    [SerializeField] private Vector3 endPoint;

    [Header("Deck Data")]
    [SerializeField] private DeckData deckData;

    [Header("List of cards on Battle")]
    [SerializeField] private List<CardData> currentDeck;
    [SerializeField] private List<CardData> currentHand;
    [SerializeField] private List<CardData> currentGraveyard;
    [SerializeField] private List<CardData> currentBanished;

    #endregion
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //SetUp();
    }

    public void SetUp()
    {
        currentDeck = new List<CardData>(deckData.DeckList);
        Debug.Log("Deck Ready");
        DrawCard(5);
    }

    public void DrawCard(int value)
    {
        for(int i = 0; i < value; i++)
        {
            if(currentDeck.Count == 0)
            {
                Debug.Log("No Deck. Shuffle Graveyard");
                ShuffleCard();
                if (currentDeck.Count == 0) { Debug.Log("No Cards in Deck & Graveyard"); break; }   //덱과 묘지 모두 빈 경우
            }

            CardData data = currentDeck[0]; //Debug.Log("Cloned card on Top :" + data.name);
            currentDeck.RemoveAt(0);    //Debug.Log("Remove from current Deck. Now Deck count :" + currentDeck.Count);
            currentHand.Add(data);      //Debug.Log("Add to Hand List. Now Hand count :" + currentHand.Count);

            GameObject cardGO = Instantiate(basicCard, spawnParent);
            cardGO.GetComponent<CardOnScene>().SetCard(data);
            Debug.Log("Draw a Card");

            SortCard();
        }
    }

    public void ShuffleCard()
    {
        currentDeck.AddRange(currentGraveyard);
        currentGraveyard.Clear();
        ShuffleDeck();
    }

    public void ShuffleDeck()
    {
        for(int i =0; i<currentDeck.Count; i++)
        {
            int randIndex = Random.Range(i, currentDeck.Count);
            (currentDeck[i], currentDeck[randIndex]) = (currentDeck[randIndex], currentDeck[i]);
        }
    }

    public void SortCard()
    {
        int count = currentHand.Count;
        if (count == 0) return;

        for(int i = 0; i<spawnParent.childCount; i++)
        {
            Transform card = spawnParent.GetChild(i);
            float t = count == 1 ? 0.5f : (float)i / (count - 1);
            Vector3 targetPosition = Vector3.Lerp(startPoint, endPoint, t);
            card.localPosition = targetPosition;
        }
    }
}
