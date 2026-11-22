using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Proto2/Character", order = 0)]

public class CharacterData : ScriptableObject
{
    [Header("Character Profile")]
    [SerializeField] private string characterName;
    [SerializeField] private string characterDescription;
    [SerializeField] private CardColor cardColor;
    [SerializeField] private int maxHealth;
    [SerializeField] private int baseAttack;
    [SerializeField] private int baseShield;
    [SerializeField] private int baseSpeed;

    [Header("Visual")]
    [SerializeField] private Sprite characterSprite;
    [SerializeField] private RuntimeAnimatorController animatorController;

    [Header("UI Setting")]
    [SerializeField] private Sprite thumbNail;
    [SerializeField] private Color uiColor;

    #region cache
    public string CharacterName => characterName;
    public string CharacterDescription => characterDescription;
    public CardColor CardColor => cardColor;
    public int MaxHealth => maxHealth;
    public int BaseAttack => baseAttack;
    public int BaseShield => baseShield;
    public int BaseSpeed => baseSpeed;
    public Sprite CharacterSprite => characterSprite;
    public RuntimeAnimatorController AnimatorController => animatorController;
    public Sprite ThumbNail => thumbNail;
    public Color UIColor => uiColor;
    #endregion
}
