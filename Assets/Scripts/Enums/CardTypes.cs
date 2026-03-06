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

    public enum CardActionType
    {
        Attack,
        Block,
        Dodge,
        Draw,
        Search,
        StatusChange,
        Buff,
        Debuff
    }

    public enum ActionTargetType
    {
        None = 0,
        Ally,
        Enemy,
        AllAlly,
        AllEnemy,
        AllCharacter,
        RandomEnemy
    }

    public enum PatternTargetType
    {
        Self,
        RandomMob,
        TurnHero,
        AllHero,
        RandomTwoHero
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
}