using Proto2.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//v0.01 / 2026.03.11 / 17:18
// 최초 생성
[Serializable]
public abstract class BuffBase 
{
    [Header("Identity")]
    [SerializeField] private BuffTypes buffType = 0;
    [SerializeField] private bool isDebuff = false;
    [SerializeField] private string buffName;
    [SerializeField] private string description;

    [Header("Duration")]
    [SerializeField] private int duration = 1;
    [SerializeField] private BuffDurationTypes durationType = 0;

    #region Cache
    public BuffTypes BuffType => buffType;
    public bool IsDebuff => isDebuff;
    public string BuffName => buffName;
    public string Description => description;
    public int Duration => duration;
    public BuffDurationTypes DurationType => durationType;
    #endregion

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
