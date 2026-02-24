using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionParameters
{
    public readonly float value;
    public readonly List<CharacterOnScene> charactersOnScene;
    public readonly List<EnemyOnScene> enemiesOnScene;

    public EnemyActionParameters(float value, List<CharacterOnScene> charactersOnScene, List<EnemyOnScene> enemiesOnScene) //Creater
    {
        this.value = value;
        this.charactersOnScene = charactersOnScene;
        this.enemiesOnScene = enemiesOnScene;
    }
}
public abstract class EnemyActionBase
{
    public EnemyActionBase() { }

    public abstract EnemyPatternType PatternType { get; }
    public abstract void DoAction(EnemyActionParameters patternParameters);
  
}

public class AttackPattern : EnemyActionBase
{
    public override EnemyPatternType PatternType => EnemyPatternType.Attack;
    public override void DoAction(EnemyActionParameters patternParameters)
    {
        //patternParameters.enemyOnScene.DoAttack();
        //patternParameters.charactersOnScene[0].currentHealth -= patternParameters.value;
        Debug.Log("Attack Action Done");
    }
}

public class DeBuffPattern : EnemyActionBase
{
    public override EnemyPatternType PatternType => EnemyPatternType.DeBuff;
    public override void DoAction(EnemyActionParameters patternParameters)
    {
        Debug.Log("Debuff Action Done");
    }
}
