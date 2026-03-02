using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class CharacterOnScene : MonoBehaviour
{
    [Header("Field")]
    [SerializeField] private CharacterData characterData;
    [SerializeField] private float currentHealth;
    [SerializeField] private float currentAttack;
    [SerializeField] private float currentShield;
    [SerializeField] private float currentSpeed;
    [SerializeField] private Transform myTransform;
    [SerializeField] private bool isTurn;
    [SerializeField] private float currentArmor;

    [Header("Visual Componenets")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    #region Cache
    public CharacterData CharacterData => characterData;
    public float CurrentHealth => currentHealth;
    public float CurrentAttack => currentAttack;
    public float CurrentShield => currentShield;
    public float CurrentSpeed => currentSpeed;
    public bool IsTurn => isTurn;
    public float CurrentArmor => currentArmor;
    #endregion

    public void SetCharacter(CharacterData chData)
    {
        characterData = chData;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        myTransform = transform.parent.GetComponent<Transform>();

        spriteRenderer.sprite = characterData.CharacterSprite;
        animator.runtimeAnimatorController = characterData.AnimatorController;
        currentHealth = characterData.MaxHealth;
        currentSpeed = characterData.BaseSpeed;
        currentAttack = characterData.BaseAttack;
        currentShield = characterData.BaseShield;
        isTurn = false;
        
    }

    public void EnterTurn()
    {
        isTurn = true;
        myTransform.localScale = new Vector3(1.2f, 1.2f, 1f);
    }

    public void ExitTurn()
    {
        isTurn = false;
        myTransform.localScale = new Vector3(0.8f, 0.8f, 1f);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            animator.SetTrigger("New Trigger");
        }

        else if(Input.GetKeyDown(KeyCode.S))
        {
            animator.SetTrigger("CardUse");
            myTransform.localScale = new Vector3(1.2f, 1.2f, 1f);
        }

        else if (Input.GetKeyDown(KeyCode.D))
        {
            animator.SetTrigger("Attack");
        }
    }

    public void AttackAnim()
    {
        animator.SetTrigger("Attack");
    }
    public void GetArmor(float value)
    {
        currentArmor = value;
    }
    public void GetDamage(float value)
    {
        currentHealth -= value;
        animator.SetTrigger("Damaged");
    }

}
