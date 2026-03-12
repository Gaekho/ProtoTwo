using Proto2.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//v0.02 / 2026.03.12 / 01:32
// BuffInstance 생성기 추가.
[Serializable]
public abstract class BuffBase 
{
    [Header("Identity")]
    [SerializeField] protected BuffTypes buffType = 0;
    [SerializeField] protected bool isDebuff = false;
    [SerializeField] protected string buffName;
    [SerializeField] protected string description;
    [SerializeField] protected BuffTriggerTiming triggerTiming = 0;

    [Header("Duration")]
    [SerializeField] protected int duration = 1;
    [SerializeField] protected ReduceTiming reduceTiming = 0;

    #region Cache
    public BuffTypes BuffType => buffType;
    public bool IsDebuff => isDebuff;
    public string BuffName => buffName;
    public string Description => description;
    public BuffTriggerTiming TriggerTiming => triggerTiming;
    public int Duration => duration;
    public ReduceTiming ReduceTiming => reduceTiming;
    #endregion

    //BuffInstance 생성기. 추가 필드를 가지는 Buff의 Instance들은 해당 필드를 가지는 전용 인스턴스를 제작한 뒤, override 해야 한다.
    public virtual BuffInstance CreateInstance(BattleUnitBase owner, BattleUnitBase applier)
    {
        return new BuffInstance(this, owner, applier);
    }  
    
    //중복 버프 부여 시도 시 작동.
    //기본적으로 지속시간만큼 연장.
    //스택이 필요한 경우 혹은 다른 로직이 필요한 경우 하위 클래스에서 override 해서 사용.
    public virtual void MergeToSameBuff(BuffInstance originalBuff, BattleUnitBase newApplier)
    {
        originalBuff.ProlongDuration(this.duration);
        originalBuff.SetNewApplier(newApplier);
    }

    //버프가 실제로 추가된 직후 1회 호출
    public virtual void OnApply(BuffInstance instance) { }

    //버프 제거 직전 1회 호출
    public virtual void OnRemove(BuffInstance instance) { }

    //소유자 턴 시작 시 호출
    public virtual void OnTurnStart(BuffInstance instance) { }

    //소유자 턴 종료 시 호출
    public virtual void OnTurnEnd(BuffInstance instance) { }

}
