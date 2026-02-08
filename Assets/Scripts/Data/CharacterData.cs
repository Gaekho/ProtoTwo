using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Proto2/Character", order = 0)]

public class CharacterData : ScriptableObject
{
    [Header("Character Profile")]
    [SerializeField] private string characterName;
    [SerializeField] private string characterDescription;
    [SerializeField] private CardColor cardColor;
    [SerializeField] private float maxHealth;
    [SerializeField] private float baseAttack;
    [SerializeField] private float baseShield;
    [SerializeField] private float baseSpeed;

    [Header("Visual")]
    [SerializeField] private Sprite characterSprite;
    [SerializeField] private AnimatorController animatorController;

    #region cache
    public string CharacterName => characterName;
    public string CharacterDescription => characterDescription;
    public CardColor CardColor => cardColor;
    public float MaxHealth => maxHealth;
    public float BaseAttack => baseAttack;
    public float BaseShield => baseShield;
    public float BaseSpeed => baseSpeed;
    public Sprite CharacterSprite => characterSprite;
    public AnimatorController AnimatorController => animatorController;
    #endregion
}
