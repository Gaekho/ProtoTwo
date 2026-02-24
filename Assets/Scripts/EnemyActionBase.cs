using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionParameters
{
    public readonly float value;
    public readonly CharacterOnScene characterOnScene;
    public readonly EnemyOnScene enemyOnScene;

    public EnemyActionParameters(float value, CharacterOnScene characterOnScene, EnemyOnScene enemyOnScene) //Creater
    {
        this.value = value;
        this.characterOnScene = characterOnScene;
        this.enemyOnScene = enemyOnScene;
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
        patternParameters.enemyOnScene.DoAttack();
        patternParameters.characterOnScene.currentHealth -= patternParameters.value;
    }
}
