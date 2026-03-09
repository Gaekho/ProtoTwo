using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proto2.Enums;
using System;

public class PatternActionParameters
{
    public readonly EnemyUnit owner;
    public readonly BattleUnitBase target;

    public readonly EnemyPatternData patternData;

    public PatternActionParameters(EnemyUnit owner, BattleUnitBase target, EnemyPatternData patternData)
    {
        this.owner = owner;
        this.target = target;
        this.patternData = patternData;
    }
}

[Serializable]
public abstract class PatternActionBase 
{
    [SerializeField] protected ActionTargetType actionTarget;

    public abstract void DoAction(PatternActionParameters actionParameters);

    protected List<BattleUnitBase> ActionTargets(PatternActionParameters actionParameters)
    {
        List<BattleUnitBase> result = new();

        switch (actionTarget)
        {
            case ActionTargetType.Owner:
                result.Add(actionParameters.owner);
                break;

            case ActionTargetType.SelectedTarget:
                if(actionParameters.target != null)
                {
                    result.Add(actionParameters.target);
                }
                else
                {
                    result.Add(BattleManager.Instance.TurnCharacter);
                }
                break;

            case ActionTargetType.AllUnits:
                result.AddRange(BattleManager.Instance.PlayerParty);
                result.AddRange(BattleManager.Instance.EnemyList);
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
public class AttackPatternAction : PatternActionBase
{
    [SerializeField] private float damage = 1f;

    public override void DoAction(PatternActionParameters actionParameters)
    {
        foreach (BattleUnitBase target in ActionTargets(actionParameters))
        {
            if(target == null) continue;
            target.GetDamage(damage);
        }
    }
}

[Serializable]
public class AddArmorPatternAction : PatternActionBase
{
    [SerializeField] private float armorAmount = 1f;

    public override void DoAction(PatternActionParameters actionParameters)
    {
        foreach(BattleUnitBase target in ActionTargets(actionParameters))
        {
            if(target == null) continue;
            target.AddArmor(armorAmount);
        }
    }
}
