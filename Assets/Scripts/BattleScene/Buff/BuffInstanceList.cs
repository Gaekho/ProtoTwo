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
    [SerializeField] private int changedAmount;

    public StatChangeBuffInstance(BuffBase sourceBuff, BattleUnitBase owner, BattleUnitBase applier, ConditionType stat) : base(sourceBuff, owner, applier)
    {
        this.stat = stat;
        this.changedAmount = 0;
    }

    public void SetAmount(int newAmount)
    {
        changedAmount = newAmount;
    }

    public void MergeStatChange(int amount)
    {
        changedAmount += amount;
    }
    public int GetChangedAmount()
    {
        return changedAmount;
    }
}
