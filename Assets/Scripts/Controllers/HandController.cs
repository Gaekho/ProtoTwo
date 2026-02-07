using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    //private HandController() { }
    public static HandController Instance { get; private set; }

    [Header("Card Prefab")]
    public GameObject basicCard;

    [Header("Hand Position")]
    public Vector3 startPoint;
    public Vector3 endPoint;

    [Header("Deck Data")]
    public DeckData deckData;

    [Header("List of cards on Battle")]
    public List<CardData> currentDeck;
    public List<CardData> currentHand;
    public List<CardData> currentGraveyard;
    public List<CardData> currentBanished;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetUp();
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

            CardData data = currentDeck[0]; Debug.Log("Cloned card on Top :" + data.name);
            currentDeck.RemoveAt(0);    Debug.Log("Remove from current Deck. Now Deck count :" + currentDeck.Count);
            currentHand.Add(data);      Debug.Log("Add to Hand List. Now Hand count :" + currentHand.Count);

            GameObject cardGO = Instantiate(basicCard, transform);
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

        for(int i = 0; i<transform.childCount; i++)
        {
            Transform card = transform.GetChild(i);
            float t = count == 1 ? 0.5f : (float)i / (count - 1);
            Vector3 targetPosition = Vector3.Lerp(startPoint, endPoint, t);
            card.localPosition = targetPosition;
        }
    }
}
