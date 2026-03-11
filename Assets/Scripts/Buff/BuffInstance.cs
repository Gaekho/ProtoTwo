using Proto2.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//v0.01 / 2026.03.11 / 17:18
// 置段 持失
[Serializable]
public class BuffInstance 
{
    [SerializeReference] private BuffBase sourceBuff;

    [SerializeField] private BattleUnitBase owner;
    [SerializeField] private BattleUnitBase applier;
    [SerializeField] private int remainTurn;

    #region Cache
    public BuffBase SourceBuff => sourceBuff;
    public BattleUnitBase Owner => owner;
    public BattleUnitBase Applier => applier;
    public int RemainTurn => remainTurn;
    #endregion

    public BuffInstance (BuffBase sourceBuff, BattleUnitBase owner, BattleUnitBase applier)
    {
        this.sourceBuff = sourceBuff;
        this.owner = owner;
        this.applier = applier;
        remainTurn = sourceBuff.Duration;
    }


    public void SetNewApplier(BattleUnitBase newApplier)
    {
        applier = newApplier;
    }

    public void ProlongDuration(int amount)
    {
        remainTurn += amount;
    }

    public void ReduceBuffDuration(BuffDurationTypes durationTypes)
    {
        if (sourceBuff.DurationType == durationTypes) remainTurn--;
    }
}
