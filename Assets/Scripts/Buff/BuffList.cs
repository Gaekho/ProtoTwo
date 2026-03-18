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

//[Serializable]
//public class BufferBuff : BuffBase
//{
//    [SerializeField] private 
//}