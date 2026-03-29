//v0.02 / 2026.03.07 / 21:07
namespace Proto2.Enums
{
    public enum UnitTeam
    {
        Ally, 
        Enemy, 
        Neutral
    }
    public enum CardType
    {
        None = 0,
        Action,
        Reaction,
        Ritual
    }

    public enum ConditionType
    {
        Attack,
        Shield,
        Speed
    }

    public enum CardColor
    {
        None = 0,
        Red, 
        Blue, 
        Green,
        Gray
    }

    public enum CardAnimTrigger
    {
        Attack,
        AddArmor,
        ApplyBuff,
        ApplyDebuff,
        ReceiveBuff,
        ReceiveDebuff,
        //이 아래로는 카드 고유
        Draw
    }

    public enum EnemyPatternAnimTrigger
    {
        Attack,
        AddArmor,
        ApplyBuff,
        ApplyDebuff,
        ReceiveBuff,
        ReceiveDebuff
    }

    public enum ActionTargetType
    {
        None = 0,
        Owner,
        SelectedTarget,
        AllAllies,
        AllEnemies,
        AllUnits,
        RandomEnemy,
        RandomAlly
        //Random two 추가 할 수도 있음.
    }

    public enum CardTargetType
    {
        None = 0,
        Ally,
        Enemy,
        AllAlly,
        AllEnemy,
        AllCharacter,
        RandomEnemy
    }

    public enum EnemyPatternType
    {
        Attack, 
        Block, 
        Dodge, 
        Buff,
        DeBuff
    }

    public enum TurnState
    {
        None,
        AllyTurn,
        EnemyTurn,
        End
    }

    public enum BranchActionCondition
    {
        None = 0,

        //체력 비교
        OwnerHealthGreater,
        OwnerHealthLess,
        TargetHealthGreater,
        TargetHealthLess,

        //버프 유무
        OwnerHasBuff,
        OwnerNotHasBuff,
        TargetHasBuff,
        TargetNotHasBuff
    }

}