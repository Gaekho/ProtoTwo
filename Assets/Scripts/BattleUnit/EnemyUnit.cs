using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proto2.Enums;

//v0.03 / 2026.03.20 / 17:25
//변경 요약 : UI Controller 추가
public class EnemyUnit : BattleUnitBase
{
    #region Field
    [Header("Enemy Unit")]
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private float baseSpeed => enemyData.BaseSpeed;

    [Header("EnemyUI")]
    [SerializeField] private EnemyUIController uiController;

    [Header("Current Pattern")]
    [SerializeField] private EnemyPatternData currentPattern;
    #endregion

    public EnemyData EnemyData => enemyData;
    public override float CurrentSpeed => baseSpeed;

    public void SetProfile(EnemyData enemyData)
    {
        this.enemyData = enemyData;
        base.SetProfile(UnitTeam.Enemy, enemyData.MaxHealth);
        mySprite.sprite = enemyData.EnemySprite;

        uiController.SetUpUI(this);

        //패턴 세팅
        SetRandomPattern();
    }

    #region Overrides
    public override void GetDamage(float value)
    {
        base.GetDamage(value);
        uiController.SetArmorText(currentArmor);
        uiController.SetHealth(currentHealth);
    }

    public override void AddArmor(float value)
    {
        base.AddArmor(value);
        uiController.SetArmorText(currentArmor);
    }

    public override void ReceiveBuff(BuffBase buff, BattleUnitBase applier)
    {
        // 1. 버프 리스트에 인스턴스 추가
        BuffInstance alreadyExist = buffList.Find(x => x.SourceBuff.BuffType == buff.BuffType);

        if (alreadyExist == null)
        {
            BuffInstance newInstance = buff.CreateInstance(this, applier);
            buffList.Add(newInstance);
            buff.OnApply(newInstance);
            uiController.CreateOrRefreshBuffUI(newInstance);
        }
        else
        {
            buff.MergeToSameBuff(alreadyExist, applier);
            buff.OnApply(alreadyExist);
            uiController.CreateOrRefreshBuffUI(alreadyExist);
        }

        // 2. 애니메이션 재생
        if (applier != this)
        {
            if (buff.IsDebuff) DoReceiveDebuffAnim();
            else DoReceiveBuffAnim();
        }
    }
    protected override IEnumerator Die()
    {
        BattleManager.Instance.EnemyDead(this);       //에러 발생으로 잠시 주석처리. EnemyDead 수정 후 다시 사용.
        DoDieAnim();
        yield return null;                             //사망 애니메이션 발동 후 한 프레임 보장을 통해 버그 가능성 낮추기.
        yield return new WaitForSeconds(myAnimator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(0.2f);
        Destroy(transform.parent.gameObject);
    }

    //Gpt가 짜준 Die 루틴. WaitUntil을 사용해서 애니메이션 종료를 감지한다.
    //protected override IEnumerator Die()
    //{
    //    if (myAnimator != null)
    //    {
    //        myAnimator.SetTrigger("Die");

    //        yield return new WaitUntil(() =>
    //            !myAnimator.IsInTransition(0) &&
    //            myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Die"));

    //        yield return new WaitUntil(() =>
    //            !myAnimator.IsInTransition(0) &&
    //            myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Die") &&
    //            myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
    //    }

    //    if (transform.parent != null)
    //        Destroy(transform.parent.gameObject);
    //    else
    //        Destroy(gameObject);
    //}
    #endregion

    #region Methods
    public void SetRandomPattern()
    {
        int index = Random.Range(0, enemyData.PatternList.Count);
        currentPattern = enemyData.PatternList[index];
        uiController.SetPatternImage(currentPattern);
    }

    public void UsePattern()
    {
        switch (currentPattern.PatternType)
        {
            case EnemyPatternAnimTrigger.Attack:
                DoAttackAnim(); break;

            case EnemyPatternAnimTrigger.AddArmor:
                DoArmorAnim(); break;

            case EnemyPatternAnimTrigger.ApplyBuff:
                DoApplyBuffAnim(); break;

            case EnemyPatternAnimTrigger.ApplyDebuff:
                DoApplyDebuffAnim(); break;
        }

        foreach(PatternActionBase patternAction in currentPattern.PatternActionList)
        {
            patternAction.DoAction(new PatternActionParameters(this, BattleManager.Instance.TurnCharacter, currentPattern));
        }
    }

    public IEnumerator UsePatternRoutine()
    {
        switch (currentPattern.PatternType)
        {
            case EnemyPatternAnimTrigger.Attack:
                DoAttackAnim();
                //yield return WaitForAnimationStateEnd("Attack");
                break;

            case EnemyPatternAnimTrigger.AddArmor:
                DoArmorAnim();
                //yield return WaitForAnimationStateEnd("AddArmor");
                break;

            case EnemyPatternAnimTrigger.ApplyBuff:
                DoApplyBuffAnim();
                //yield return WaitForAnimationStateEnd("ApplyBuff");
                break;

            case EnemyPatternAnimTrigger.ApplyDebuff:
                DoApplyDebuffAnim();
                //yield return WaitForAnimationStateEnd("ApplyDebuff");
                break;
        }

        foreach (PatternActionBase patternAction in currentPattern.PatternActionList)
        {
            patternAction.DoAction(new PatternActionParameters(this, BattleManager.Instance.TurnCharacter, currentPattern));
        }

        yield return new WaitForSeconds(0.7f);
    }

    #endregion
}
