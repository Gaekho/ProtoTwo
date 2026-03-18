using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proto2.Enums;
using System;

//v0.02 / 2026.03.07 / 22:53
//변경 요약: 카드 타겟, 액션 타겟 분리, SerializeReference CardActionBase 리스트
[CreateAssetMenu(fileName = "Card Data", menuName = "Proto2/Card/baseCard", order = 0)]

public class CardData : ScriptableObject
{
    //[Header("Card Profile")]
    [SerializeField] private int id;
    [SerializeField] private string cardName;
    [SerializeField] private CardType type;
    [SerializeField] private CardColor color;
    [SerializeField] private string cardText;

    //[Header("visual")]
    [SerializeField] private Sprite cardSprite;
    [SerializeField] private Sprite dragIcon;
    [SerializeField] private CardAnimTrigger cardAnimTrigger;

    //[Header("Active Condition")]
    [SerializeField] private bool usableWithoutTarget;
    [SerializeField] private CardTargetType cardTarget;
    [SerializeField] private List<ActiveConditionData> activeConditionList;

    //[Header("After Used")]
    [SerializeField] private bool banishAfterUsed;
    //[Header("Action List")]
    [SerializeReference] 
    private List<CardActionBase> cardActionList;

    #region Cache
    public int Id => id;
    public string CardName => cardName;
    public CardType Type => type;
    public CardColor Color => color;
    public string CardText => cardText;
    public Sprite CardSprite => cardSprite;
    public Sprite DragIcon => dragIcon;
    public CardAnimTrigger CardAnimTrigger => cardAnimTrigger;
    public CardTargetType CardTarget => cardTarget;
    public List<ActiveConditionData> ActiveConditionList => activeConditionList;
    public bool UsableWithoutTarget => usableWithoutTarget;
    public bool BanishAfterUsed => banishAfterUsed;
    public List<CardActionBase> CarActionList => cardActionList;
    #endregion
}

//[Serializable]
//public class CardActionData
//{
//    [SerializeField] private CardActionType cardActionType;
//    //[SerializeField] private ActionTargetType actionTargetType;
//    [SerializeField] private float actionValue;

//    public CardActionType CardActionType => cardActionType;
//    //public ActionTargetType ActionTargetType => actionTargetType; 
//    public float ActionValue => actionValue;
//}

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