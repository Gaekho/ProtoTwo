using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proto2.Enums;

//v0.03 / 2026.03.12 / 00:37
// 변경 요약 : ReceiveBuff 수정, TriggerBuff 추가
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
        // <Summary>
        // 1. 버프 리스트에 인스턴스 추가
        //      중복된 버프가 있는지 검사
        //            없을 경우 : 그냥 추가하면 된다 아님? => BuffList.Add로 그냥 추가
        //            있을 경우 : 추가가 아니라 지속 시간 & 부여자 갱신 => buffInstance.MergeToSame(x)
         
        // 2. 애니메이션 재생
        //      부여자랑 수여자가 같은 경우 : 애니메이션 재생 스킵
        //      다를경우 : 지금 여기서 받는 모션 재생
        //

        // 1. 버프 리스트에 인스턴스 추가
        BuffInstance alreadyExist = buffList.Find(x => x.SourceBuff.BuffType == buff.BuffType);

        if(alreadyExist == null)
        {
            BuffInstance newInstance = buff.CreateInstance(this, applier);
            buffList.Add(newInstance);
            buff.OnApply(newInstance);
        }
        else
        {
            buff.MergeToSameBuff(alreadyExist, applier);
            buff.OnApply(alreadyExist);
        }

        // 2. 애니메이션 재생
        if(applier != this)
        {
            if (buff.IsDebuff) DoReceiveDebuffAnim();
            else DoReceiveBuffAnim();
        }
    }

    public void TriggerBuff(BuffTriggerTiming timing)
    {
        List<BuffInstance> snapshot = new(buffList);

        foreach (BuffInstance buff in snapshot)
        {
            if(buff == null || buff.SourceBuff == null) continue;
            if (!buffList.Contains(buff)) continue;

            // 1.효과 발동
            if(timing == BuffTriggerTiming.OnTurnStart && buff.SourceBuff.TriggerTiming == BuffTriggerTiming.OnTurnStart)
            {
                buff.SourceBuff.OnTurnStart(buff);
            }
            if(timing == BuffTriggerTiming.OnTurnEnd && buff.SourceBuff.TriggerTiming == BuffTriggerTiming.OnTurnEnd)
            {
                buff.SourceBuff.OnTurnEnd(buff);
            }

            // 2. 지속시간 감소
            if (timing == BuffTriggerTiming.OnTurnStart && buff.SourceBuff.ReduceTiming == ReduceTiming.StartOfOwnerTurn)
            {
                buff.ReduceBuffDuration();
            }

            if (timing == BuffTriggerTiming.OnTurnEnd && buff.SourceBuff.ReduceTiming == ReduceTiming.EndOfOwnerTurn)
            {
                buff.ReduceBuffDuration();
            }
        }
    }
    public void RemoveBuff(BuffInstance buff)
    {
        int idx = buffList.IndexOf(buff);
        if(idx < 0) return;

        buffList.RemoveAt(idx);
    }

    #region Animation Wait
    public IEnumerator WaitForAnimationStateEnd(string stateName, int layer = 0)
    {
        if (myAnimator == null) yield break;

        yield return null;

        // 1. 해당 상태에 실제로 들어갈 때까지 대기
        yield return new WaitUntil(() =>
            myAnimator.GetCurrentAnimatorStateInfo(layer).IsName(stateName));

        // 2. 상태가 시작된 뒤 한 프레임 더 보장
        yield return null;

        // 3. 그 상태가 끝나서 다른 상태로 넘어갈 때까지 대기
        yield return new WaitUntil(() =>
            !myAnimator.GetCurrentAnimatorStateInfo(layer).IsName(stateName));
    }
    #endregion

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
