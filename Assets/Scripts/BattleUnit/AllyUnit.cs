using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//V0.01 / 2026.03.07 / 18:31
public class AllyUnit : BattleUnitBase
{
    [Header("Ally Unit")]
    [SerializeField] private CharacterData characterData;
    [SerializeField] private float currentAttack;
    [SerializeField] private float currentShield;
    [SerializeField] private float currentSpeed;
    [SerializeField] private bool isTurn;
    [SerializeField] private Transform myTransform;     //턴 교체 시 크기 변경용


    #region Cache
    public CharacterData CharacterData => characterData;
    public float CurrentAttack => currentAttack;
    public float CurrentShield => currentShield;
    public float CurrentSpeed => currentSpeed;
    public bool IsTurn => isTurn;
    #endregion

    public void SetProfile(CharacterData characterData)
    {
        this.characterData = characterData;
        base.SetProfile(UnitTeam.Ally, characterData.MaxHealth);
        mySprite.sprite = characterData.CharacterSprite;
        
        //스탯 저장
        currentAttack = characterData.BaseAttack;
        currentShield = characterData.BaseShield;
        currentSpeed = characterData.BaseSpeed;
        
        //턴 세팅
        isTurn = false;
        myTransform = transform.parent;
        myTransform.localScale = new Vector3(0.8f, 0.8f, 1f);
    }

    public override void GetDamage(float value)
    {
        base.GetDamage(value);
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
    protected override IEnumerator Die()
    {
        //아직 미구현. 애니메이션 재생 및 자기 자신 파괴, BattleManager의 AllyList에서 삭제 등의 작업 추가.
        yield break;
    }

    #region Animation Triggers
    public void DoApplyBuffAnim()
    {
        myAnimator.SetTrigger("CardUse");
    }
    #endregion
}
