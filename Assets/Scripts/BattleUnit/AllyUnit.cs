using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//V0.01 / 2026.03.07 / 18:31
public class AllyUnit : BattleUnitBase
{
    #region Field
    [Header("Ally Unit")]
    [SerializeField] private CharacterData characterData;
    [SerializeField] private float currentAttack;
    [SerializeField] private float currentShield;
    [SerializeField] private float currentSpeed;
    [SerializeField] private bool isTurn;
    [SerializeField] private Transform myTransform;     //턴 교체 시 크기 변경용
    [SerializeField] private AllyUnitUIcontroller uiController;
    #endregion

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
        myAnimator.runtimeAnimatorController = characterData.AnimatorController;

        //스탯 저장
        currentAttack = characterData.BaseAttack;
        currentShield = characterData.BaseShield;
        currentSpeed = characterData.BaseSpeed;
        
        //턴 세팅
        isTurn = false;
        myTransform = transform.parent;
        myTransform.localScale = new Vector3(0.8f, 0.8f, 1f);

        //UI 세팅
        uiController.SetUIC(this);
    }

    #region Overrides
    public override void GetDamage(float value)
    {
        base.GetDamage(value);
        //UIManager 연결 후에 슬라이더 표시 기능 구현
        uiController.SetArmorAmount(currentArmor);
        uiController.SetHealthSlider(currentHealth);
    }

    public override void AddArmor(float value)
    {
        base.AddArmor(value);
        uiController.SetArmorAmount(currentArmor);
    }

    public override void StatusChange(ConditionType statType, float amount)
    {
        switch (statType)
        {
            case ConditionType.Attack:
                currentAttack += amount;    break;
            case ConditionType.Shield:
                currentShield += amount;    break;
            case ConditionType.Speed:
                currentSpeed += amount;     break;
        }
        uiController.SetStatTexts(currentAttack, currentShield, currentSpeed);
    }
    public override void ReceiveBuff(BuffBase buff, BattleUnitBase applier)
    {
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

    public override void RemoveBuff(BuffInstance buff)
    {
        if (buff == null || buff.SourceBuff == null) return;

        BuffTypes type = buff.SourceBuff.BuffType;
        base.RemoveBuff(buff);
        uiController.RemoveBuffUI(type);
    }
    protected override IEnumerator Die()
    {
        //아직 미구현. 애니메이션 재생 및 자기 자신 파괴, BattleManager의 AllyList에서 삭제 등의 작업 추가.
        yield break;
    }
    #endregion

    #region Methods
    public void EnterTurn()
    {
        isTurn = true;
        myTransform.localScale = new Vector3(1.2f, 1.2f, 1f);
        uiController.SetTurn(true);
    }
    
    public void ExitTurn()
    {
        isTurn = false;
        myTransform.localScale = new Vector3(0.8f, 0.8f, 1f);
        uiController.SetTurn(false);
    }

    public void RefreshBuffUI()
    {
        uiController.RefreshAllBuffUI(buffList);
    }
    #endregion

    #region Original Animation Triggers
    public void DoDrawAnim()
    {
        myAnimator.SetTrigger("Draw");
    }
    #endregion
}
