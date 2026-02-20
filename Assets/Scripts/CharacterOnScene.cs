using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class CharacterOnScene : MonoBehaviour
{
    [SerializeField] private CharacterData characterData;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public CharacterData CharacterData => characterData;
    public float currentHealth;
    public float currentGnosis;
    public float currentSpeed;
    public float currentAttack;
    public float currentShield;
    public bool isTurn;
    public Transform myTransform;

    
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


}
