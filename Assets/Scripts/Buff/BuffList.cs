using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using Proto2.Enums;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

// v0.01 / 2026.03.12 / 01:04
// 置段 持失
[Serializable]
public class TauntBuff : BuffBase
{
    public TauntBuff() 
    {
        buffType = BuffTypes.Taunt;
        isDebuff = false;
        buffName = "Taunt";
        description = "Get Enemies Attack or Apply to this Unit";
        triggerTiming = BuffTriggerTiming.None;

        duration = 1;
        reduceTiming = ReduceTiming.EndOfOwnerTurn;
    }
    

    public override void OnApply(BuffInstance instance)
    {
        Debug.Log($"{instance.Owner.name} gained Taunt.");
    }
}

[Serializable]
public class StatPlusBuff : BuffBase
{
    [SerializeField] ConditionType stat;
    [SerializeField] float plusAmount;

    public ConditionType Stat => stat;
    public float PlusAmount => plusAmount;

    public StatPlusBuff()
    {
        buffType = BuffTypes.StatPlus;
        isDebuff = false;
        buffName = "Stat Plus";
        description = "Stat bonus as amount, while duration";
        triggerTiming = BuffTriggerTiming.None;

        duration = 1;
        reduceTiming = ReduceTiming.EndOfOwnerTurn;
        stat = ConditionType.Attack;
        plusAmount = 1;
    }

    public override void OnApply(BuffInstance instance)
    {
        instance.Owner.StatusChange(stat ,plusAmount);
    }

    public override void OnRemove(BuffInstance instance)
    {
        instance.Owner.StatusChange(stat, -plusAmount);
    }

    public override void UpdateTooltip()
    {
        description = $"{stat} Stat bonus as {plusAmount}, while duration";
    }
}

[Serializable]
public class StrengthenBuff : BuffBase
{
    [SerializeField] private float additionalDamageRate;

    public StrengthenBuff()
    {
        buffType = BuffTypes.Strengthen;
        isDebuff = false;
        buffName = "Strengthen";
        description = $"Attack damage increase {(int)(additionalDamageRate*100)}% while duration.";
        triggerTiming = BuffTriggerTiming.None;

        duration = 1;
        reduceTiming = ReduceTiming.EndOfOwnerTurn;

        additionalDamageRate = 1.1f;
    }

    public override BuffInstance CreateInstance(BattleUnitBase owner, BattleUnitBase applier)
    {
        return new StrengthenBuffInstance(this, owner, applier, additionalDamageRate);
    }

    public override void MergeToSameBuff(BuffInstance originalBuff, BattleUnitBase newApplier)
    {
        base.MergeToSameBuff(originalBuff, newApplier);

        StrengthenBuffInstance inst = originalBuff as StrengthenBuffInstance;
        if (inst == null) return;

        inst.SetDamageRate(additionalDamageRate);
    }

    public override void UpdateTooltip()
    {
        description = $"Attack damage increase {(int)(additionalDamageRate * 100)}% while duration.";
    }

}