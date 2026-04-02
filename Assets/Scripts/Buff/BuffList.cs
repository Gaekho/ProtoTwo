using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using Proto2.Enums;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

// v0.01 / 2026.03.12 / 01:04
// 최초 생성
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
public class AtkStatPlusBuff : BuffBase
{
    [SerializeField] ConditionType stat;
    [SerializeField] float plusAmount;

    public ConditionType Stat => stat;
    public float PlusAmount => plusAmount;

    public AtkStatPlusBuff()
    {
        buffType = BuffTypes.AtkStatPlus;
        isDebuff = false;
        buffName = "공격 스탯 강화";
        description = $"{duration}턴 동안 공격 스탯이 {plusAmount}증가한다.";
        triggerTiming = BuffTriggerTiming.None;

        duration = 1;
        reduceTiming = ReduceTiming.EndOfOwnerTurn;
        stat = ConditionType.Attack;
        plusAmount = 1;
    }

    public override void OnApply(BuffInstance instance)
    {
        instance.Owner.StatusChange(ConditionType.Attack ,plusAmount);
    }

    public override void OnRemove(BuffInstance instance)
    {
        instance.Owner.StatusChange(ConditionType.Attack, -plusAmount);
    }

    public override void UpdateTooltip()
    {
        description = $"{duration}턴 동안 공격 스탯이 {plusAmount}증가한다.";
    }
}

[Serializable]
public class ShdStatPlusBuff : BuffBase
{
    [SerializeField] ConditionType stat;
    [SerializeField] float plusAmount;

    public ConditionType Stat => stat;
    public float PlusAmount => plusAmount;

    public ShdStatPlusBuff()
    {
        buffType = BuffTypes.ShdStatPlus;
        isDebuff = false;
        buffName = "방어 스탯 강화";
        description = $"{duration}턴 동안 방어 스탯이 {plusAmount}증가한다.";
        triggerTiming = BuffTriggerTiming.None;

        duration = 1;
        reduceTiming = ReduceTiming.EndOfOwnerTurn;
        stat = ConditionType.Shield;
        plusAmount = 1;

    }
    public override void OnApply(BuffInstance instance)
    {
        instance.Owner.StatusChange(ConditionType.Shield, plusAmount);
    }

    public override void OnRemove(BuffInstance instance)
    {
        instance.Owner.StatusChange(ConditionType.Shield, -plusAmount);
    }

    public override void UpdateTooltip()
    {
        description = $"{duration}턴 동안 방어 스탯이 {plusAmount}증가한다.";
    }

}

[Serializable]
public class SpdStatPlusBuff : BuffBase
{
    [SerializeField] ConditionType stat;
    [SerializeField] float plusAmount;

    public ConditionType Stat => stat;
    public float PlusAmount => plusAmount;

    public SpdStatPlusBuff()
    {
        buffType = BuffTypes.SpdStatPlus;
        isDebuff = false;
        buffName = "속도 스탯 강화";
        description = $"{duration}턴 동안 속도 스탯이 {plusAmount}증가한다.";
        triggerTiming = BuffTriggerTiming.None;

        duration = 1;
        reduceTiming = ReduceTiming.EndOfOwnerTurn;
        stat = ConditionType.Speed;
        plusAmount = 1;
    }

    public override void OnApply(BuffInstance instance)
    {
        instance.Owner.StatusChange(ConditionType.Speed, plusAmount);
    }

    public override void OnRemove(BuffInstance instance)
    {
        instance.Owner.StatusChange(ConditionType.Speed, -plusAmount);
    }

    public override void UpdateTooltip()
    {
        description = $"{duration}턴 동안 속도 스탯이 {plusAmount}증가한다.";
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