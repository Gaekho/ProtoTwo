using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Proto2.Enums;

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
        reduceTiming = ReduceTiming.EndOfOwnerTurn;
    }

    public override void OnApply(BuffInstance instance)
    {
        Debug.Log($"{instance.Owner.name} is Poisoned.");
    }

    public override void OnTurnStart(BuffInstance instance)
    {
        float damage = instance.Owner.CurrentHealth * 0.1f;
        if (damage > 30) damage = 30;
        instance.Owner.GetDamage(damage);
    }

    public override void UpdateTooltip()
    {
        description = $"턴 시작 시 최대 체력의 10% 피해 (최대 30)을 입는다.";
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

    public override void UpdateTooltip()
    {
        description = $"턴 시작 시 {duration} 피해를 입는다.";
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
        reduceTiming = ReduceTiming.EndOfOwnerTurn;
    }

    public override void OnApply(BuffInstance instance)
    {
        instance.Owner.StatusChange(ConditionType.Speed, -1);
    }

    public override void OnRemove(BuffInstance instance)
    {
        instance.Owner.StatusChange(ConditionType.Speed, 1);
    }

    public override void UpdateTooltip()
    {
        description = "지속시간동안 속도 스탯이 1 감소한다.";
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
        totalDamage /= totalDamage/2;
        instance.Owner.GetDamage(totalDamage);
    }

    public override void UpdateTooltip()
    {
        //description = $"Damage {}to owner when removed.";
    }
}
