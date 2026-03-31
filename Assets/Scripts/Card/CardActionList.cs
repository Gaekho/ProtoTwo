using JetBrains.Annotations;
using Proto2.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackAction : CardActionBase
{
    [SerializeField] private float damage = 1f;
    public override void DoAction(CardActionParameters actionParameters)
    {
        foreach (BattleUnitBase target in ActionTargets(actionParameters))
        {
            float totalDamage = damage;
            if (actionParameters.owner.HasBuff(BuffTypes.Strengthen))
            {
                //actionParameters.owner.GetBuff(BuffTypes.Strengthen) as
            }

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
        foreach (BattleUnitBase target in ActionTargets(actionParameters))
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
        foreach (BattleUnitBase target in ActionTargets(actionParameters))
        {
            target.ReceiveBuff(buff, actionParameters.owner);
        }
    }
}

#region Branching Action Supporter
[Serializable]
public class BranchConditionData
{
    [SerializeField] private BranchActionCondition condition;
    [SerializeField] private float healthRate;
    [SerializeField] private BuffTypes buffType;

    public BranchActionCondition Condition => condition;
    public float HealthRate => healthRate;
    public BuffTypes BuffType => buffType;
}
#endregion
[Serializable]
public class BranchingAction : CardActionBase
{
    [Header("Condition")]
    [SerializeField] private BranchConditionData condition = new();

    [Header("Actions If Ture")]
    [SerializeReference] private List<CardActionBase> trueActions = new();

    [Header("Actions If False")]
    [SerializeReference] private List<CardActionBase> falseActions = new();
     
    public override void DoAction(CardActionParameters actionParameters)
    {
        switch(BranchConditionCheck(condition, actionParameters))
        {
            case true:
                foreach (CardActionBase Taction in trueActions) Taction.DoAction(actionParameters);
                break;

            case false:
                foreach (CardActionBase Faction in falseActions) Faction.DoAction(actionParameters);
                break;
        }
    }
    private bool BranchConditionCheck(BranchConditionData branchCondition, CardActionParameters actionParameters)
    {
        switch (branchCondition.Condition)
        {            
            case BranchActionCondition.OwnerHealthGreater:
                if (actionParameters.owner.CurrentHealth >= branchCondition.HealthRate) return true;
                else return false;

            case BranchActionCondition.OwnerHealthLess:
                if (actionParameters.owner.CurrentHealth <= branchCondition.HealthRate) return true;
                else return false;
            
            case BranchActionCondition.TargetHealthGreater:
                if (actionParameters.target.CurrentHealth >= branchCondition.HealthRate) return true;
                else return false;

            case BranchActionCondition.TargetHealthLess:
                if (actionParameters.target.CurrentHealth <= branchCondition.HealthRate) return true;
                else return false;

            case BranchActionCondition.OwnerHasBuff:
                if (actionParameters.owner.HasBuff(branchCondition.BuffType)) return true;
                else return false;

            case BranchActionCondition.OwnerNotHasBuff:
                if (actionParameters.owner.HasBuff(branchCondition.BuffType)) return false;
                else return true;

            case BranchActionCondition.TargetHasBuff:
                if (actionParameters.target.HasBuff(branchCondition.BuffType)) return true;
                else return false;

            case BranchActionCondition.TargetNotHasBuff:
                if (actionParameters.target.HasBuff(branchCondition.BuffType)) return false;
                else return true;
        }
        return false;
    }
}

[Serializable]
public class ClearArmorAction : CardActionBase
{
    public override void DoAction(CardActionParameters actionParameters)
    {
        foreach(BattleUnitBase target in ActionTargets(actionParameters))
        {
            target.ClearArmor();
        } 
    }
}

[Serializable]
public class RemoveBuffAction : CardActionBase
{
    [SerializeReference] private BuffBase targetBuff;

    public override void DoAction(CardActionParameters actionParameters)
    {
        foreach(BattleUnitBase target in ActionTargets(actionParameters))
        {
            if (target.HasBuff(targetBuff.BuffType))
            {
                BuffInstance removeTarget = target.GetBuff(targetBuff.BuffType);
                target.RemoveBuff(removeTarget);
            }
        }
    }

    [Serializable]
    public class ShowIntentAction : CardActionBase
    {
        public override void DoAction(CardActionParameters actionParameters)
        {
            foreach (BattleUnitBase target in ActionTargets(actionParameters))
            {
                EnemyUnit enemy = target as EnemyUnit;
            }
        }
    }
}