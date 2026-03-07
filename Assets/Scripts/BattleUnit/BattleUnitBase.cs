using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proto2.Enums;

//v0.01 / 2026.03.06 / 16:41
public abstract class BattleUnitBase : MonoBehaviour
{
    [Header("Battle Unit")]
    [SerializeField] protected UnitTeam team;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float currentArmor;
    [SerializeField] protected bool isDead;

    [Header("Visual Components")]
    [SerializeField] protected SpriteRenderer mySprite;
    [SerializeField] protected Animator myAnimator;

    #region Cache
    public UnitTeam Team => team;
    public float CurrentHealth => currentHealth;
    public float CurrentArmor => currentArmor;
    public bool IsDead => isDead;
    #endregion

    public virtual void SetProfile(UnitTeam myTeam, float maxHealth)
    {
        team = myTeam;
        isDead = false;
        currentHealth = maxHealth;
        currentArmor = 0f;
        mySprite = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
    }

    public virtual void AddArmor(float value)
    {
        if (value <= 0) return;
        currentArmor += value;
    }

    public virtual void ClearArmor()
    {
        currentArmor = 0f;
    }

    public virtual void GetDamage(float value)
    {
        if (value <= 0) return;
        float remainDamage = value;

        if(currentArmor > 0f) 
        {
            float absorbed = Mathf.Min(remainDamage, currentArmor);
            remainDamage -= absorbed;
            currentArmor -= absorbed;
        }

        if(remainDamage > 0f)
        {
            currentHealth -= remainDamage;
        }

        if (currentHealth <= 0f && !isDead)
        {
            currentHealth = 0f;
            isDead = true;
            StartCoroutine(Die());
        }
    }

    protected abstract IEnumerator Die();
}
