using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckData", menuName = "Proto2/Deck", order = 0)]

public class DeckData : ScriptableObject
{
    [Header("Deck Profile")]
    [SerializeField] private string deckName = "나만의 XX 덱";

    [Header("Deck List")]
    [SerializeField] private List<CardData> deckList;

    #region cache
    public string DeckName => deckName;
    public List<CardData> DeckList => deckList;
    #endregion
}
