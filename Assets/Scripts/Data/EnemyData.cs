using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using UnityEngine.UIElements;

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

    #region cache
    public string CharacterName => enemyName;
    public string CharacterDescription => enemyDescription;
    public float MaxHealth => maxHealth;
    public float BaseSpeed => baseSpeed;
    public Sprite CharacterSprite => characterSprite;
    public AnimatorController AnimatorController => animatorController;
    #endregion
}
