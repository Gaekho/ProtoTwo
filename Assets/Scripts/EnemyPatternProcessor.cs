using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyPatternProcessor
{
    private static readonly Dictionary<EnemyPatternType, EnemyActionBase> enemyActionDict = new(); //new Dictionary< ... , ...>

    public static void Initialize()
    {
        enemyActionDict.Clear();
        enemyActionDict.Add(EnemyPatternType.Attack, new AttackPattern());

        // Add additional PatternType here whenever developed New Pattern Actions
    }

    public static EnemyActionBase GetPattern(EnemyPatternType patternType)
    {
        return enemyActionDict[patternType];
    }
}