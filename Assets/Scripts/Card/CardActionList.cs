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

[Serializable]
public class BranchingAction : CardActionBase
{
     
    public override void DoAction(CardActionParameters actionParameters)
    {
        throw new NotImplementedException();
    }
}