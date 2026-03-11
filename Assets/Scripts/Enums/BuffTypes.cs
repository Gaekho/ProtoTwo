//v0.01 / 2026.03.11 / 17:18
// 최초 생성

namespace Proto2.Enums
{
    public enum BuffTypes
    {
        None = 0,
        //버프
        Taunt,
        Regen,
        Addiction,
       
        //디버프
        Weakness,
        Vulnerable,
        Poison,
    }

    public enum BuffDurationTypes
    {
        None = 0,

        // 소유자 턴 시작 시 duration 감소
        StartOfOwnerTurn,

        // 소유자 턴 종료 시 duration 감소
        EndOfOwnerTurn,

        // 라운드 종료 시 duration 감소
        EndOfRound,

        // 영구 지속
        Permanent
    }

    public enum BuffTriggerTiming
    {
        None = 0,
        OnApply,
        OnRemove,
        OnTurnStart,
        OnTurnEnd
    }
}