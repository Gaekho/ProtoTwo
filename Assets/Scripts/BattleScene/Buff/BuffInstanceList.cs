using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Proto2.Enums;

[Serializable]
public class StrengthenBuffInstance : BuffInstance
{
    [SerializeField] private float damageIncreaseRate;

    public float DamageIncreaseRate => damageIncreaseRate;

    public StrengthenBuffInstance(BuffBase sourcebuff, BattleUnitBase owner, BattleUnitBase applier, float damageRate) : base(sourcebuff, owner, applier)
    {
        this.damageIncreaseRate = damageRate;
    }

    public void SetDamageRate(float rate)
    {
        this.damageIncreaseRate = rate;
    }
}

[Serializable]
public class StatChangeBuffInstance : BuffInstance
{
    [SerializeField] private ConditionType stat;
    [SerializeField] private int amount;

    public StatChangeBuffInstance(BuffBase sourceBuff, BattleUnitBase owner, BattleUnitBase applier, ConditionType stat, int plusAmount) : base(sourceBuff, owner, applier)
    {
        this.stat = stat;
        this.amount = plusAmount;
    }

    public void SetAmount(int newAmount)
    {
        amount = newAmount;
    }
    public int GetAmount()
    {
        return amount;
    }
}
