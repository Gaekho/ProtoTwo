using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionParameters
{
    public readonly float value;
    public readonly CharacterOnScene characterOnScene;
    public readonly EnemyOnScene enemyOnScene;
    public readonly CardData cardData;
    public readonly CardOnScene cardOnScene;

    public CardActionParameters(float value, CharacterOnScene characterOnScene, EnemyOnScene enemyOnScene, CardData cardData, CardOnScene cardOnScene)
    {
        this.value = value;
        this.characterOnScene = characterOnScene;
        this.enemyOnScene = enemyOnScene;
        this.cardData = cardData;
        this.cardOnScene = cardOnScene;
    }
}
public abstract class CardActionBase 
{
    public CardActionBase() { }
    public abstract CardActionType ActionType { get; }
    public abstract void DoAction(CardActionParameters actionParameters);
}

public class AttackAction : CardActionBase
{
    public override CardActionType ActionType => CardActionType.Attack;
    public override void DoAction(CardActionParameters actionParameters)
    {
        actionParameters.characterOnScene.AttackAnim();
        actionParameters.enemyOnScene.GetDamage(actionParameters.value);
    }
}
