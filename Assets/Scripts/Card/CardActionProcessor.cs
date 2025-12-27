using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardActionProcessor 
{
    private static readonly Dictionary<CardActionType, CardActionBase> CardActionDict = new Dictionary<CardActionType, CardActionBase>();

    public static void Initialize()
    {
        CardActionDict.Clear();
        CardActionDict.Add(CardActionType.Attack, new AttackAction());

    }

    public static CardActionBase GetAction(CardActionType actionType)
    {
        return CardActionDict[actionType];
    }
}
