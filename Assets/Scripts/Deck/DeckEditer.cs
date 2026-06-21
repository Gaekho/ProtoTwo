using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckEditer : MonoBehaviour
{
    [Header("Deck")]
    [SerializeField] private DeckData deckData; //현재 덱
    [SerializeField] private List<CardData> wholeCard; //전체 카드(없어도 됨)
    [SerializeField] private List<CardData> ownedCard; //보유한 카드

    public void AddCard(CardData newCard) { ownedCard.Add(newCard); }

    // Start is called before the first frame update
    void Start()
    {
        deckData = GameManager.Instance.GetPlayerDeck();
    }


}
