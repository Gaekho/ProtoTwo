using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEventsLibrary
{
    // Root UI
    public static event Action OnFadeOutDone;
    public static event Action OnFadeInDone;

    // Scene Flow

    // Battle
    public static event Action OnBattleStart;
    public static event Action OnBattleEnd;

    public static event Action<BattleUnitBase> OnUnitTurnStart;
    public static event Action OnUnitTurnEnd;
    public static event Action<AllyUnit> OnActingRightShifted;
    public static event Action<AllyUnit, int, int, int> OnUnitStatChanged;

    // **********************
    // *****Raise Events*****
    // **********************

    // Root UI
    public static void RaiseFadeOutDone() => OnFadeOutDone?.Invoke();
    public static void RaiseFadeInDone() => OnFadeInDone?.Invoke();

    // Battle
    public static void RaiseBattleStart() => OnBattleStart?.Invoke();
    public static void RaiseBattleEnd() => OnBattleEnd?.Invoke();

    public static void RaiseUnitTurnStart(BattleUnitBase unit) => OnUnitTurnStart?.Invoke(unit);
    public static void RaiseUnitTurnEnd() => OnUnitTurnEnd?.Invoke();
    public static void RaiseActingRightShifted(AllyUnit holder) => OnActingRightShifted?.Invoke(holder);
    public static void RaiseUnitStatChanged(AllyUnit unit, int atk, int shd, int spd) => OnUnitStatChanged?.Invoke(unit, atk, shd, spd);
    
}
