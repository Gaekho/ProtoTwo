using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proto2.Enums;

//v0.01 / 2026.03.07 / 07:29
public class EnemyUnit : BattleUnitBase
{
    #region Field
    [Header("Enemy Unit")]
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private Canvas myCanvas;
    [SerializeField] private Slider healthSlider;
    #endregion

    public void SetProfile(EnemyData enemyData)
    {
        this.enemyData = enemyData;
        base.SetProfile(UnitTeam.Enemy, enemyData.MaxHealth);
        mySprite.sprite = enemyData.EnemySprite;

        //UI Canvas 세팅
        myCanvas = transform.parent.GetComponentInChildren<Canvas>();
        healthSlider = myCanvas.GetComponentInChildren<Slider>();
        healthSlider.value = 1f;

        //패턴 세팅
    }

    #region Overrides
    public override void GetDamage(float value)
    {
        base.GetDamage(value);
        healthSlider.value = currentHealth / enemyData.MaxHealth;
    }
    protected override IEnumerator Die()
    {
        BattleManager.Instance.EnemyDead(this);       //에러 발생으로 잠시 주석처리. EnemyDead 수정 후 다시 사용.
        DoDieAnim();
        yield return new WaitForSeconds(2f);
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
    #endregion
}
