using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proto2.Enums;
using System;

[CreateAssetMenu(fileName = "Card Data", menuName = "Proto2/Card", order = 0)]

public class CardData : ScriptableObject
{
    [Header("Card Profile")]
    [SerializeField] private string id;
    [SerializeField] private string cardName;
    [SerializeField] private CardType type;
    [SerializeField] private CardColor color;
    [SerializeField] private Sprite cardSprite;

    [Header("Active Condition")]
    [SerializeField] private List<ActiveConditionData> activeConditionList;

    [Header("Action Settings")]
    [SerializeField] private bool usableWithoutTarget;
    [SerializeField] private List<CardActionData> cardActionDataList;

    #region Cache
    public string Id => id;
    public string CardName => cardName;
    public CardType Type => type;
    public CardColor Color => color;
    public Sprite CardSprite => cardSprite;
    public List<ActiveConditionData> ActiveConditionList => activeConditionList;
    public bool UsableWithoutTarget => usableWithoutTarget;
    public List<CardActionData> CarActionDataList => cardActionDataList;
    #endregion
}

[Serializable]
public class CardActionData
{
    [SerializeField] private CardActionType cardActionType;
    [SerializeField] private ActionTargetType actionTargetType;
    [SerializeField] private float actionValue;

    public CardActionType CardActionType => cardActionType;
    public ActionTargetType ActionTargetType => actionTargetType; 
    public float ActionValue => actionValue;
}

[Serializable]
public class ActiveConditionData
{
    [SerializeField] private ConditionType condition;
    [SerializeField] private float value;
}