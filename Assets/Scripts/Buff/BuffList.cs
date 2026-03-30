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
public class PoisonDebuff : BuffBase
{
    public PoisonDebuff() 
    {
        buffType = BuffTypes.Poison;
        isDebuff = true;
        buffName = "Poison";
        description = $"Get 10% of current health (Max 30) damages on start of turn.";
        triggerTiming = BuffTriggerTiming.OnTurnStart;

        duration = 1;
        reduceTiming= ReduceTiming.EndOfOwnerTurn;
    }

    public override void OnApply(BuffInstance instance)
    {
        Debug.Log($"{instance.Owner.name} is Poisoned.");
    }

    public override void OnTurnStart(BuffInstance instance)
    {
        float damage = instance.Owner.CurrentHealth * 0.1f;
        if(damage > 30) damage = 30;
        instance.Owner.GetDamage(damage);
    }
}

[Serializable]
public class BleedingDebuff : BuffBase
{
    public BleedingDebuff()
    {
        buffType = BuffTypes.Bleeding;
        isDebuff = true;
        buffName = "Bleeding";
        description = $"Get {duration} damages on start of turn.";
        triggerTiming = BuffTriggerTiming.OnTurnStart;

        duration = 1;
        reduceTiming = ReduceTiming.EndOfOwnerTurn;
    }
    public override void OnTurnStart(BuffInstance instance)
    {
        instance.Owner.GetDamage(duration);
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
}

[Serializable]
public class OilDebuff : BuffBase
{
    public OilDebuff()
    {
        buffType = BuffTypes.Oil;
        isDebuff = true;
        buffName = "Oil";
        description = "Speed -1 while duration";
        triggerTiming = BuffTriggerTiming.None;

        duration = 1;
        reduceTiming= ReduceTiming.EndOfOwnerTurn;
    }

    public override void OnApply(BuffInstance instance)
    {
        instance.Owner.StatusChange(ConditionType.Speed, -1);
    }

    public override void OnRemove(BuffInstance instance)
    {
        instance.Owner.StatusChange(ConditionType.Speed, 1);
    }
}

[Serializable]
public class UnstableDebuff : BuffBase
{
    [SerializeField] private int amount;
    public UnstableDebuff()
    {
        buffType = BuffTypes.Unstable;
        isDebuff = true;
        buffName = "Unstable";
        description = "Damage to owner when removed";
        triggerTiming = BuffTriggerTiming.OnRemove;

        duration = 1;
        reduceTiming = ReduceTiming.Permanent;
    }

    public override void OnRemove(BuffInstance instance)
    {
        int totalDamage;
        totalDamage = instance.RemainTurn * (instance.RemainTurn + 3);
        totalDamage /= 2;
        instance.Owner.GetDamage(totalDamage);
    }

}