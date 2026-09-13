using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
