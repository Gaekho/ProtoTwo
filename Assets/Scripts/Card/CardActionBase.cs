using Proto2.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//v0.03 / 2026.03.07 / 21:39
//변경 요약 : CardActionParameters의 필드를 readonly 형태로 변경, Serializable 추가.
public class CardActionParameters
{
    public readonly BattleUnitBase owner;
    public readonly BattleUnitBase target;

    public readonly CardData cardData;
    public readonly CardInstance cardInstance;
    public readonly CardOnScene cardOnScene;

    public CardActionParameters(BattleUnitBase owner, BattleUnitBase target, CardData cardData, CardInstance cardInstance, CardOnScene cardOnScene)
    {
        this.owner = owner;
        this.target = target;
        this.cardData = cardData;
        this.cardInstance = cardInstance;
        this.cardOnScene = cardOnScene;
    }
}

[Serializable]
public abstract class CardActionBase 
{
    [SerializeField] protected ActionTargetType actionTarget;
    public abstract void DoAction(CardActionParameters actionParameters);

    protected List<BattleUnitBase> ActionTargets(CardActionParameters actionParameters)
    {
        List<BattleUnitBase> result = new();

        switch (actionTarget)
        {
            case ActionTargetType.Owner:
                result.Add(actionParameters.owner);
                break;

            case ActionTargetType.SelectedTarget:
                result.Add(actionParameters.target);
                break;
            //BattleManager 수정 후에 나머지 만들기(PlayerParty, EnemyList 등의 수정 이후
        }
        return result;
    }
}

[Serializable]
public class AttackAction : CardActionBase
{
    [SerializeField] private float damage = 1f;
    public override void DoAction(CardActionParameters actionParameters)
    {
        foreach(BattleUnitBase target in ActionTargets(actionParameters))
        {
            target.GetDamage(damage);
        }
    }
}

[Serializable]
public class DrawAction : CardActionBase
{
    [SerializeField] private int drawCount = 1;
    public override void DoAction(CardActionParameters actionParameters)
    {
        HandController.Instance.DrawCard(drawCount);
    }
}

[Serializable]
public class HealAction : CardActionBase
{
    [SerializeField] private float healAmount = 1f;
    public override void DoAction(CardActionParameters actionParameters)
    {
        //actionParameters.characterOnScene.CurrentHealth += actionParameters.value;
    }
}

[Serializable]
public class AddArmorAction : CardActionBase
{
    [SerializeField] private float armorAmount = 1f;

    public override void DoAction(CardActionParameters actionParameters)
    {
        foreach(BattleUnitBase target in ActionTargets(actionParameters))
        {
            target.AddArmor(armorAmount);
        }
    }
}
