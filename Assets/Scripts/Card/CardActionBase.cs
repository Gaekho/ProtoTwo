using Proto2.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//v0.04 / 2026.03.12 / 01:02
//변경 요약 : Enum 스위치문 완성.
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

            case ActionTargetType.AllAllies:
                result.AddRange(BattleManager.Instance.PlayerParty);
                break;

            case ActionTargetType.AllEnemies:
                result.AddRange(BattleManager.Instance.EnemyList);
                break;

            case ActionTargetType.RandomAlly:
                int i = UnityEngine.Random.Range(0, BattleManager.Instance.PlayerParty.Count);
                result.Add(BattleManager.Instance.PlayerParty[i]);        //한줄로 쓰면 가독성 개떨어질까봐 두줄로 씀.
                break;

            case ActionTargetType.RandomEnemy:
                int j = UnityEngine.Random.Range(0, BattleManager.Instance.EnemyList.Count);
                result.Add(BattleManager.Instance.EnemyList[j]);
                break;
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

[Serializable]
public class ApplyBuffAction : CardActionBase
{
    [SerializeReference] private BuffBase buff;

    public override void DoAction(CardActionParameters actionParameters)
    {
        foreach(BattleUnitBase target in ActionTargets(actionParameters))
        {
            target.ReceiveBuff(buff, actionParameters.owner);
        }
    }
}