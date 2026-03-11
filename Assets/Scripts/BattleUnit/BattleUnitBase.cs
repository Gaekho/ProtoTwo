using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proto2.Enums;

//v0.02 / 2026.03.11 / 17:18
// 변경 요약 : 버프 리스트 및 ReceiveBuff 추가
public abstract class BattleUnitBase : MonoBehaviour
{
    #region Field
    [Header("Battle Unit")]
    [SerializeField] protected UnitTeam team;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float currentArmor;
    [SerializeField] protected bool isDead;
    [SerializeReference] protected List<BuffInstance> buffList = new();

    [Header("Visual Components")]
    [SerializeField] protected SpriteRenderer mySprite;
    [SerializeField] protected Animator myAnimator;
    #endregion

    #region Cache
    public UnitTeam Team => team;
    public float CurrentHealth => currentHealth;
    public float CurrentArmor => currentArmor;
    public bool IsDead => isDead;
    #endregion

    #region Virtual Methods
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
        DoArmorAnim();
    }

    public virtual void ClearArmor()
    {
        currentArmor = 0f;
    }

    public virtual void GetDamage(float value)
    {
        float previousHealth = currentHealth;           //데미지 받기 전 체력 저장.

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

        if (!isDead && previousHealth > currentHealth)       //함수 발동 전 체력과 발동 후 체력 비교를 통해 실제 체력 손실이 있을때만 피격 애니메이션 재생.
        {
            DoDamagedAnim();
        }
    }

    public virtual void ReceiveBuff( BuffBase buff, BattleUnitBase applier)
    {
        /*
         * 1. 버프 리스트에 인스턴스 추가
         *      중복된 버프가 있는지 검사
                    없을 경우 : 그냥 추가하면 된다 아님? => BuffList.Add로 그냥 추가
                    있을 경우 : 추가가 아니라 지속 시간 & 부여자 갱신 => buffInstance.MergeToSame(x)
         
         * 2. 애니메이션 재생
         *      부여자랑 수여자가 같은 경우 : 애니메이션 재생 스킵
         *      다를경우 : 지금 여기서 받는 모션 재생
         */

        //버프 리스트에 인스턴스 추가
        BuffInstance alreadyExist = buffList.Find(x => x.SourceBuff.BuffType == buff.BuffType);

        if(alreadyExist == null)
        {
            buffList.Add(new BuffInstance(buff, this, applier));
            //buffInstance.SourceBuff.OnApply(buffInstance);
        }
        else
        {
            buff.MergeToSameBuff(alreadyExist, applier);
        }

        //애니메이션 재생
        if(applier != this)
        {
            if (buff.IsDebuff) DoReceiveDebuffAnim();
            else DoReceiveBuffAnim();
        }
    }

    #endregion

    #region Abstract Methods
    protected abstract IEnumerator Die();
    #endregion

    #region Animation Trigger
    public virtual void DoAttackAnim()
    {
        myAnimator.SetTrigger("Attack");
    }
 
    public virtual void DoArmorAnim()
    {
        myAnimator.SetTrigger("AddArmor");
    }

    public virtual void DoApplyBuffAnim()
    {
        myAnimator.SetTrigger("ApplyBuff");
    }
    public virtual void DoReceiveBuffAnim()
    {
        myAnimator.SetTrigger("ReceiveBuff");
    }

    public virtual void DoApplyDebuffAnim()
    {
        myAnimator.SetTrigger("ApplyDebuff");
    }

    public virtual void DoReceiveDebuffAnim()
    {
        myAnimator.SetTrigger("ReceiveDebuff");
    }
 
    public virtual void DoDamagedAnim()
    {
        myAnimator.SetTrigger("Damaged");
    }

    protected virtual void DoDieAnim()
    {
        myAnimator.SetTrigger("Die");
    }
    #endregion
}
