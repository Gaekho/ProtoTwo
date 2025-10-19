using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOnScene : MonoBehaviour
{
    [SerializeField] private CharacterData characterData;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public float currentHealth;
    public float currentGnosis;
    public float currentSpeed;
    public float currentAttack;
    public float currentShield;
    public bool isTurn = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        spriteRenderer.sprite = characterData.CharacterSprite;
        animator.runtimeAnimatorController = characterData.AnimatorController;
        currentHealth = characterData.MaxHealth;
        currentSpeed = characterData.BaseSpeed;
        currentAttack = characterData.BaseAttack;
        currentShield = characterData.BaseShield;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            animator.SetTrigger("New Trigger");
        }

        else if(Input.GetKeyDown(KeyCode.V))
        {
            animator.SetTrigger("CardUse");
        }
    }
}
