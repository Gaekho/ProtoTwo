using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using Proto2.Enums;

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
        description = $"Get {duration} damages on start of turn.";
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
        instance.Owner.GetDamage(duration);
    }
}