using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using Proto2.Enums;

//v0.02 / 2026.03.09 / 16:01
[CreateAssetMenu(fileName = "Enemy Data", menuName = "Proto2/Enemy", order = 0)]

public class EnemyData : ScriptableObject
{
    //[Header("Character Profile")]
    [SerializeField] private string enemyName;
    [SerializeField] private string enemyDescription;
    [SerializeField] private float maxHealth;
    [SerializeField] private float baseSpeed;

    //[Header("Visual")]
    [SerializeField] private Sprite enemySprite;
    [SerializeField] private RuntimeAnimatorController animatorController;

    //[Header("Patterns")]
    [SerializeField] private List<EnemyPatternData> patternList = new();

    #region cache
    public string EnemyName => enemyName;
    public string EnemyDescription => enemyDescription;
    public float MaxHealth => maxHealth;
    public float BaseSpeed => baseSpeed;
    public Sprite EnemySprite => enemySprite;
    public RuntimeAnimatorController AnimatorController => animatorController;
    public List<EnemyPatternData> PatternList => patternList;
    #endregion
}

[Serializable]
public class EnemyPatternData
{
    [SerializeField] private string patternName;
    [SerializeField] private EnemyPatternAnimTrigger patternType;  //애니메이션 재생용
    [SerializeField] private Sprite intentIcon;

    [SerializeReference] 
    private List<PatternActionBase> patternActionList = new();

    #region Cache
    public string PatternName => patternName;
    public EnemyPatternAnimTrigger PatternType => patternType; //애니메이션 재생용
    public Sprite IntentIcon => intentIcon;
    public List<PatternActionBase> PatternActionList => patternActionList;
    #endregion
}

//[Serializable]
//public class EnemyPatternAction
//{
//    [SerializeField] private EnemyPatternType patternActionType;
//    [SerializeField] private float patternValue;
//    [SerializeField] private PatternTargetType actionTargetType;

//    public EnemyPatternType PatternActionType => patternActionType;
//    public float PatternValue => patternValue;
//    public PatternTargetType ActionTargetType => actionTargetType;
//}