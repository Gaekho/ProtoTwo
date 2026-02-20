namespace Proto2.Enums
{
    public enum CardType
    {
        None = 0,
        Action,
        Reaction,
        Ritual
    }

    public enum ConditionType
    {
        Health,
        Gnosis,
        Attack,
        Shield,
        Speed
    }

    public enum CardColor
    {
        None = 0,
        Red, 
        Blue, 
        Green
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
        FrontHero,
        MiddleHero,
        BackHero,
        TurnHero,
        AllHero
    }

    public enum EnemyPatternType
    {
        Attack, 
        Block, 
        Dodge, 
        Buff,
        DeBuff
    }
}