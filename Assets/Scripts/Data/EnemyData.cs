using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using UnityEngine.UIElements;
using System;
using Proto2.Enums;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "Proto2/Enemy", order = 0)]


public class EnemyData : ScriptableObject
{
    [Header("Character Profile")]
    [SerializeField] private string enemyName;
    [SerializeField] private string enemyDescription;
    [SerializeField] private float maxHealth;
    [SerializeField] private float baseSpeed;

    [Header("Visual")]
    [SerializeField] private Sprite enemySprite;
    [SerializeField] private AnimatorController animatorController;

    [Header("Patterns")]
    [SerializeField] private List<EnemyPatternData> patternList;

    #region cache
    public string EnemyName => enemyName;
    public string EnemyDescription => enemyDescription;
    public float MaxHealth => maxHealth;
    public float BaseSpeed => baseSpeed;
    public Sprite EnemySprite => enemySprite;
    public AnimatorController AnimatorController => animatorController;
    public List<EnemyPatternData> PatternList => patternList;
    #endregion
}

[Serializable]
public class EnemyPatternData
{
    [SerializeField] private string patternName;
    [SerializeField] private EnemyPatternType patternType;  //for Animation
    //[SerializeField] private PatternTargetType patternTargetType;
    [SerializeField] private Sprite patternImage;
    [SerializeField] private List<EnemyPatternAction> actionList;

    public string PatternName => patternName;
    public EnemyPatternType PatternType => patternType; //for Animation of a pattern.
    //public PatternTargetType PatternTargetType => patternTargetType;
    public Sprite PatternImage => patternImage;
    public List<EnemyPatternAction> ActionList => actionList;
}

[Serializable]
public class EnemyPatternAction
{
    [SerializeField] private EnemyPatternType patternActionType;
    [SerializeField] private float patternValue;
    [SerializeField] private PatternTargetType actionTargetType;

    public EnemyPatternType PatternActionType => patternActionType;
    public float PatternValue => patternValue;
    public PatternTargetType ActionTargetType => actionTargetType;
}