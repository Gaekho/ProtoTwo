using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//v0.02 / 2026.03.07 / 09:58
//변경 요약 : CharacterOnScene, EnemyOnScene을 AllyUnit, EnemyUnit으로 변경하는 작업 진행.
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

    [Header("Characters On Scene")]
    [SerializeField] private GameObject basicCharacter;
    [SerializeField] private Transform allyContainer;
    [SerializeField] private Transform enemyContainer;
    [SerializeField] private List<AllyUnit> playerParty;
    [SerializeField] private List<EnemyUnit> enemyList;

    [Header("Turn")]
    [SerializeField] private int turn = 0;
    [SerializeField] private Queue<CharacterOnScene> turnQ;
    #endregion

    public AllyUnit TurnCharacter { private set; get; }
    public TurnState CurrentState { private set; get; }
    public IReadOnlyList<AllyUnit> PlayerParty => playerParty;
    public IReadOnlyList<EnemyUnit > EnemyList => enemyList;
    public UnityEvent onTurnStart;
    
    private void Awake()
    {
        Instance = this;
        CardActionProcessor.Initialize();
        EnemyPatternProcessor.Initialize();
        SetAlly();
        Debug.Log(playerParty[0].CharacterData.name);
        TurnCharacter = playerParty[0];
        SetEnemy();
        HandController.Instance.SetUp(deckData);
        turn = 0;
    }
    private void Start()
    {
        StartCoroutine(BattleRoutine());
    }

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
            //enemies[i].SetProfile()???        //레벨 데이터를 통한 에너미 데이터 전달 시 기능하도록 구현.
        }
    }

    public void EnemyDead(EnemyOnScene dead)
    {
        int idx = enemyList.FindIndex(x => x.gameObject == dead.gameObject);
        enemyList.RemoveAt(idx);

        if(enemyList.Count == 0)
        {
            StartCoroutine(UIManager.Instance.BattleEnd("승리"));
        }
    }

    private IEnumerator BattleRoutine()
    {
        //메인 전투 반복문 시작
        while (true)
        {
            //아군 턴 시작 페이즈
            turn++;
            CurrentState = TurnState.AllyTurn;
            yield return UIManager.Instance.StartCoroutine(UIManager.Instance.TurnStart(turn, "Ally"));
            HandController.Instance.DrawCard(3);

            //플레이어 카드 사용 페이즈
            while (CurrentState == TurnState.AllyTurn)
            {

                yield return null;
            }

            //아군 턴 엔드 페이즈
            yield return UIManager.Instance.StartCoroutine(UIManager.Instance.TurnEnd());
            yield return new WaitForSeconds(0.5f);  //캐릭터 교체 후 딜레이

            
            //적 턴 시작 페이즈
            turn++;
            yield return UIManager.Instance.StartCoroutine(UIManager.Instance.TurnStart(turn, "Enemy"));

            //적 패턴 플레이 페이즈
            foreach (EnemyUnit enemy in enemyList)
            {
                yield return new WaitForSeconds(0.5f);
                //yield return enemy.StartCoroutine(enemy.UsePatternRoutine());         //적 패턴 기능 EnemyUnit에 구현 후에 다시 주석 해제.
                //enemy.SetRandomPattern();
            }
            yield return new WaitForSeconds(0.7f);  //모든 패턴 사용 후 딜레이
            
            //적 턴 종료 (이후 작업 x)
        }
    }

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


    // Update is called once per frame
    void Update()
    {
        
    }
}
