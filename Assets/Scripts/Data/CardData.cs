using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proto2.Enums;
using System;

[CreateAssetMenu(fileName = "Card Data", menuName = "Proto2/Card/baseCard", order = 0)]

public class CardData : ScriptableObject
{
    [Header("Card Profile")]
    [SerializeField] private int id;
    [SerializeField] private string cardName;
    [SerializeField] private CardType type;
    [SerializeField] private CardColor color;

    [Header("visual")]
    [SerializeField] private Sprite cardSprite;
    [SerializeField] private Sprite dragIcon;

    [Header("Active Condition")]
    [SerializeField] private ActionTargetType actionTargetType;
    [SerializeField] private List<ActiveConditionData> activeConditionList;

    [Header("Action Settings")]
    [SerializeField] private bool usableWithoutTarget;
    [SerializeField] private bool banishAfterUsed;
    [SerializeField] private List<CardActionData> cardActionDataList;

    #region Cache
    public int Id => id;
    public string CardName => cardName;
    public CardType Type => type;
    public CardColor Color => color;
    public Sprite CardSprite => cardSprite;
    public Sprite DragIcon => dragIcon;
    public ActionTargetType ActionTargetType => actionTargetType;
    public List<ActiveConditionData> ActiveConditionList => activeConditionList;
    public bool UsableWithoutTarget => usableWithoutTarget;
    public bool BanishAfterUsed => banishAfterUsed;
    public List<CardActionData> CarActionDataList => cardActionDataList;
    #endregion
}

[Serializable]
public class CardActionData
{
    [SerializeField] private CardActionType cardActionType;
    //[SerializeField] private ActionTargetType actionTargetType;
    [SerializeField] private float actionValue;

    public CardActionType CardActionType => cardActionType;
    //public ActionTargetType ActionTargetType => actionTargetType; 
    public float ActionValue => actionValue;
}

[Serializable]
public class ActiveConditionData
{
    [SerializeField] private ConditionType condition;
    [SerializeField] private float value;

    #region cache
    public ConditionType Condition => condition;
    public float Value => value;
    #endregion
}