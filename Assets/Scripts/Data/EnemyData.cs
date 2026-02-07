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
    [SerializeField] private Sprite characterSprite;
    [SerializeField] private AnimatorController animatorController;

    [Header("Patterns")]
    [SerializeField] private List<EnemyPatternData> patternList;

    #region cache
    public string CharacterName => enemyName;
    public string CharacterDescription => enemyDescription;
    public float MaxHealth => maxHealth;
    public float BaseSpeed => baseSpeed;
    public Sprite CharacterSprite => characterSprite;
    public AnimatorController AnimatorController => animatorController;
    public List<EnemyPatternData> PatternList => patternList;
    #endregion
}

[Serializable]
public class EnemyPatternData
{
    [SerializeField] private string patternName;
    [SerializeField] private EnemyPatternType patternType;
    [SerializeField] private float value;

    public string PatternName => patternName;
    public EnemyPatternType PatternType => patternType;
    public float Value => value;
}