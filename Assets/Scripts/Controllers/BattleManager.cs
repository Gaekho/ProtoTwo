using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

//v0.02 / 2026.03.12 / 02:58
//변경 요약 : BuffHook 추가, 라운드 루틴에 버프 추가.
public class BattleManager : MonoBehaviour
{
    #region Singleton
    private BattleManager() { }
    public static BattleManager Instance {  get; private set; }
    #endregion

    #region Field
    [Header("Setting")]
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private HandController handContoller;
    [SerializeField] private DeckData deckData;

    [Header("Units On Scene")]
    [SerializeField] private GameObject basicCharacter;
    [SerializeField] private Transform allyContainer;
    [SerializeField] private Transform enemyContainer;
    [SerializeField] private List<AllyUnit> playerParty;
    [SerializeField] private List<EnemyUnit> enemyList;

    [Header("Turn")]
    [SerializeField] private int turn = 0;
    [SerializeField] private int queueCount = 0;        //큐 카운트 (큐를 채울때마다 누적)
    [SerializeField] private int totalTurnCount = 0;    // 총 몇번째 턴인지 (유닛이 턴을 시작할때마다 누적)
    [SerializeField] private Queue<BattleUnitBase> turnQ;

    [Header("Temporary Fields")]
    [SerializeField] private List<EnemyData> tempEnemies;
    #endregion

    #region Cache
    public AllyUnit TurnCharacter { private set; get; }
    public BattleUnitBase ActingUnit; //{ private set; get; }
    public BattleUnitBase CurrentTurnUnit { private set; get; }
    public TurnState CurrentState { private set; get; }
    public IReadOnlyList<AllyUnit> PlayerParty => playerParty;
    public IReadOnlyList<EnemyUnit > EnemyList => enemyList;
    public int QueueCount => queueCount;
    public int TotalTurnCount => totalTurnCount;
    public bool IsResolving {  private set; get; }
    #endregion    
    
    public UnityEvent onTurnStart;

    private void Awake()
    {
        Instance = this;

        SetAlly();
        SetEnemy();

        HandController.Instance.SetUp(deckData);

        TurnCharacter = null;
        CurrentTurnUnit = null;
        CurrentState = TurnState.None;
        queueCount = 0;
        totalTurnCount = 0;

        Debug.Log(playerParty[0].CharacterData.name);
        TurnCharacter = playerParty[0];
        TurnCharacter.EnterTurn();
        turn = 0;
    }
    private void Start()
    {
        StartCoroutine(BattleRoutineTwo());
    }

    #region Setup
    private void SetAlly()
    {
        playerParty.Clear();
        AllyUnit [] allies = allyContainer.GetComponentsInChildren<AllyUnit>(); 

        for(int i=0; i<allies.Length; i++)
        {
            playerParty.Add(allies[i]);
            allies[i].SetProfile(deckData.Characters[i]);
        }
    }

    private void SetEnemy()
    {
        enemyList.Clear();
        EnemyUnit [] enemies = enemyContainer.GetComponentsInChildren<EnemyUnit>();

        for(int i= 0; i<enemies.Length; i++)
        {
            enemyList.Add(enemies[i]);
            enemies[i].SetProfile(tempEnemies[i]);        //레벨 데이터를 통한 에너미 데이터 전달 시 기능하도록 구현.
        }
    }
    #endregion

    #region TurnQueue
    private void ReBuildTurnQueue()
    {
        //리스트 생성 및 살아있는 유닛 전부 확보
        List<BattleUnitBase> aliveUnits = new();

        foreach(var ally in playerParty)
        {
            if (ally != null && !ally.IsDead)   aliveUnits.Add(ally);
        }
        foreach(var enemy in EnemyList)
        {
            if (enemy != null && !enemy.IsDead) aliveUnits.Add(enemy);
        }
        Debug.Log(aliveUnits.Count);

        //여기에 정렬 알고리즘 투입
        // 동속 랜덤용 셔플
        for (int i = 0; i < aliveUnits.Count; i++)
        {
            int rand = Random.Range(i, aliveUnits.Count);
            (aliveUnits[i], aliveUnits[rand]) = (aliveUnits[rand], aliveUnits[i]);
        }

        aliveUnits = aliveUnits
            .OrderByDescending(GetUnitSpeed)
            .ToList();

        turnQ = new Queue<BattleUnitBase>();
        foreach (var unit in aliveUnits)
        {
            turnQ.Enqueue(unit);
            Debug.Log(unit.name);
        }

        //큐 카운트 증가
        queueCount++;

        //큐 UI 업데이트(UI Manager 호출)
        UIManager.Instance.RefreshTurnQueue(queueCount, aliveUnits);
    }

    private float GetUnitSpeed(BattleUnitBase unit)
    {
        return unit.CurrentSpeed;
    }
    #endregion

    #region StateHelper
    private bool IsBattleEnd()
    {
        bool allEnemyDead = enemyList.Count == 0 || enemyList.TrueForAll(x => x == null || x.IsDead);
        bool allAllyDead = playerParty.Count == 0 || playerParty.TrueForAll(x => x == null || x.IsDead);

        if (allEnemyDead)
        {
            CurrentState = TurnState.End;
            HandController.Instance.TurnOffHand();
            StartCoroutine(UIManager.Instance.BattleEnd("승리"));
            return true;
        }

        if(allAllyDead)
        {
            CurrentState = TurnState.End;
            HandController.Instance.TurnOffHand();
            StartCoroutine(UIManager.Instance.BattleEnd("패배"));
            return true;
        }
        return false;
    }
    #endregion

    #region Buff
    private IEnumerator BuffHookRoutine(BuffTriggerTiming timing, UnitTeam team)
    {
        switch (team)
        {
            case UnitTeam.Ally:
                List<AllyUnit> snapshotA = new(playerParty);
                foreach(AllyUnit unit in snapshotA)
                {
                    if(unit == null || unit.IsDead) continue;
                    unit.TriggerBuff(timing);
                    yield return null;
                }
                break;

            case UnitTeam.Enemy:
                List<EnemyUnit> snapshotE = new(enemyList);
                foreach (EnemyUnit unit in snapshotE)
                {
                    if (unit == null || unit.IsDead) continue;
                    unit.TriggerBuff(timing);
                    yield return null;
                }
                break;
        }
        yield return new WaitForSeconds(0.05f);
    }

    private IEnumerator UnitBuffHook(BuffTriggerTiming timing, BattleUnitBase unit)
    {
        yield return new WaitForSeconds(0.1f);
        unit.TriggerBuff(timing);
        yield return new WaitForSeconds(0.2f);
    }
    #endregion

    #region Unit Dead
    public void EnemyDead(EnemyUnit dead)
    {
        int idx = enemyList.FindIndex(x => x.gameObject == dead.gameObject);
        if (idx < 0) return;
        enemyList.RemoveAt(idx);

        //ReBuildTurnQueue();

        if(enemyList.Count == 0)
        {
            HandController.Instance.TurnOffHand();
            StartCoroutine(UIManager.Instance.BattleEnd("승리"));
        }
    }

    public void AllyDead(AllyUnit dead)
    {
        int idx = playerParty.FindIndex(x=> x.gameObject == dead.gameObject);
        if (idx < 0) return;
        playerParty.RemoveAt(idx);

        IsBattleEnd();

    }
    #endregion

    #region Resolve
    public IEnumerator ResolveRoutine(IEnumerator routine)
    {
        IsResolving = true;
        yield return routine;
        IsResolving = false;
        Debug.Log(IsResolving);
    }
    #endregion

    #region Main Routine
    private IEnumerator BattleRoutine()
    {
        //메인 전투 반복문 시작
        while (true)
        {
            //아군 턴 시작 페이즈
            turn++;
            CurrentState = TurnState.AllyTurn;
            //yield return UIManager.Instance.StartCoroutine(UIManager.Instance.TurnStart(turn, "Ally"));
            yield return StartCoroutine(ResolveRoutine(UIManager.Instance.TurnStart(turn, "Ally")));
            HandController.Instance.DrawCard(3);
            yield return StartCoroutine(ResolveRoutine(BuffHookRoutine(BuffTriggerTiming.OnTurnStart, UnitTeam.Ally)));

            //플레이어 카드 사용 페이즈
            while (CurrentState == TurnState.AllyTurn)
            {
                yield return null;
            }

            //아군 턴 엔드 페이즈
            yield return StartCoroutine(ResolveRoutine(BuffHookRoutine(BuffTriggerTiming.OnTurnEnd, UnitTeam.Ally)));
            yield return StartCoroutine(ResolveRoutine(UIManager.Instance.TurnEnd()));
            yield return new WaitForSeconds(0.5f);  //캐릭터 교체 후 딜레이

            
            //적 턴 시작 페이즈
            turn++;
            yield return StartCoroutine(ResolveRoutine(UIManager.Instance.TurnStart(turn, "Enemy")));
            yield return StartCoroutine(ResolveRoutine(BuffHookRoutine(BuffTriggerTiming.OnTurnStart, UnitTeam.Enemy)));

            //적 패턴 플레이 페이즈
            List<EnemyUnit> enemySnapshot = new(enemyList);
            foreach (EnemyUnit enemy in enemySnapshot)
            {
                yield return new WaitForSeconds(0.5f);
                yield return StartCoroutine(ResolveRoutine(enemy.UsePatternRoutine()));         //적 패턴 기능 EnemyUnit에 구현 후에 다시 주석 해제.
                enemy.SetRandomPattern();
            }
            yield return new WaitForSeconds(0.7f);  //모든 패턴 사용 후 딜레이

            //적 턴 종료
            yield return StartCoroutine(ResolveRoutine(BuffHookRoutine(BuffTriggerTiming.OnTurnEnd, UnitTeam.Enemy)));
        }
    }

    private IEnumerator BattleRoutineTwo()
    {
        //최초 턴 큐 생성
        ReBuildTurnQueue();
        Debug.Log("First Queue Created");

        // 메인 배틀 진입
        while (true)
        {
            if (IsBattleEnd()) { Debug.Log("Done"); yield break; }

            if(turnQ == null || turnQ.Count == 0)
            {
                ReBuildTurnQueue();
                Debug.Log("Rebuild Queue");
                if (turnQ.Count == 0) { Debug.Log("TurnQ count 0"); yield break; }
            }

            //캐릭터 턴 시작 : 패널 표시  --> actingUnit 저장
            totalTurnCount++;
            ActingUnit = turnQ.Dequeue();
            string name;
            if(ActingUnit.Team == UnitTeam.Ally)
            {
                CurrentState = TurnState.AllyTurn;

                if(TurnCharacter != null) { TurnCharacter.ExitTurn(); }

                AllyUnit ally = ActingUnit as AllyUnit;
                
                TurnCharacter = ally;
                TurnCharacter.EnterTurn();

                name = ally.CharacterData.CharacterName;
                yield return StartCoroutine(ResolveRoutine(UIManager.Instance.UnitTurnStart(totalTurnCount, name)));
            }

            else if(ActingUnit.Team == UnitTeam.Enemy)
            {
                CurrentState = TurnState.EnemyTurn;
                EnemyUnit enemy = ActingUnit as EnemyUnit;
                name = enemy.EnemyData.EnemyName;
                yield return StartCoroutine(ResolveRoutine(UIManager.Instance.UnitTurnStart(totalTurnCount, name)));
            }

            //버프 훅(턴 시작 시)
            yield return UnitBuffHook(BuffTriggerTiming.OnTurnStart, ActingUnit);

            //유닛 턴 시작
            //분기(적 || 아군) 
            //아군이면 카드 사용 대기 및 턴 종료까지 대기
            //적이면 패턴 쓰고 턴 종료
            if(ActingUnit.Team == UnitTeam.Ally)
            {
                HandController.Instance.DrawCard(1);
                while(CurrentState == TurnState.AllyTurn)
                {
                    yield return null;
                    //씬에서 TurnEnd 버튼 클릭 시 ChangeState( End ) 호출
                }
            }
            else if(ActingUnit.Team == UnitTeam.Enemy)
            {
                EnemyUnit enemy = ActingUnit as EnemyUnit;
                yield return new WaitForSeconds(0.5f);
                yield return StartCoroutine(ResolveRoutine(enemy.UsePatternRoutine()));
                enemy.SetRandomPattern();
            }
            // 턴 종료 버프 훅
            yield return StartCoroutine(ResolveRoutine(UnitBuffHook(BuffTriggerTiming.OnTurnEnd, ActingUnit)));

            //턴 종료되면 턴이 시작하기 전에, 큐를 확인하고 비어있으면 ReBuildQueue
            if (turnQ.Count == 1) ReBuildTurnQueue();
        }
    }
    #endregion

    #region Buttons
    //Button OnClick 함수들. ScreenCanvas의 버튼에서 참조.
    public void ChangeState(int state)
    {
        CurrentState = (TurnState)state;
        Debug.Log("Current Battle State : " + CurrentState);
    }
    public void ChangeCharacter(int i)
    {
        TurnCharacter.ExitTurn();
        TurnCharacter = playerParty[i];
        TurnCharacter.EnterTurn();
    }
    #endregion

}
