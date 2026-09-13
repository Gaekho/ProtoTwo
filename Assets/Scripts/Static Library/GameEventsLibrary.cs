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
    public static event Action OnBattleEnd;


    // **********************
    // *****Raise Events*****
    // **********************

    // Root UI
    public static void RaiseFadeOutDone() => OnFadeOutDone?.Invoke();
    public static void RaiseFadeInDone() => OnFadeInDone?.Invoke();

    // Battle
    public static void RaiseBattleEnd() { Debug.Log("OnBattleEnd Invoked");  OnBattleEnd?.Invoke(); }
}
