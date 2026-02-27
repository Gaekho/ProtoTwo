using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private List<CharacterOnScene> playerParty;
    [SerializeField] private List<EnemyOnScene> enemyList;

    [Header("Turn")]
    [SerializeField] private int turn = 0;
    [SerializeField] public CharacterOnScene TurnCharacter { private set; get; }
    [SerializeField] public TurnState CurrentState { private set; get; }
    #endregion

    public IReadOnlyList<CharacterOnScene> PlayerParty => playerParty;
    //public CharacterOnScene TurnCharatcer;
    
    private void Awake()
    {
        Instance = this;
        CardActionProcessor.Initialize();
        EnemyPatternProcessor.Initialize();
        SetAlly();
        Debug.Log(PlayerParty[0].CharacterData.name);
        TurnCharacter = PlayerParty[0];
        SetEnemy();
        HandController.Instance.SetUp();
        turn = 0;
    }

    private void SetAlly()
    {
        playerParty.Clear();
        CharacterOnScene [] allies = allyContainer.GetComponentsInChildren<CharacterOnScene>(); 

        for(int i=0; i<allies.Length; i++)
        {
            playerParty.Add(allies[i]);
            allies[i].SetCharacter(deckData.Characters[i]);
        }
    }

    private void SetEnemy()
    {
        enemyList.Clear();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject go in enemies)
        {
            enemyList.Add(go.GetComponentInChildren<EnemyOnScene>());
        }
    }

    private IEnumerator BattleRoutine()
    {
        while (true)
        {
            //Ally Turn Start
            turn++;
            CurrentState = TurnState.AllyTurn;
            yield return UIManager.Instance.StartCoroutine(UIManager.Instance.TurnStart(turn, "Ally"));

            while (CurrentState == TurnState.AllyTurn)
            {
                yield return null;
            }
            //Ally Turn End
            yield return UIManager.Instance.StartCoroutine(UIManager.Instance.TurnEnd());
            yield return new WaitForSeconds(0.5f);
            
            //Enemy Turn Start
            turn++;
            yield return UIManager.Instance.StartCoroutine(UIManager.Instance.TurnStart(turn, "Enemy"));
            foreach (EnemyOnScene enemy in enemyList)
            {
                yield return new WaitForSeconds(0.7f);
                enemy.UsePattern();
                enemy.SetRandomPattern();
            }

            //Enemy Turn End
        }
        yield break;
    }

    //Button OnClick Functions on Screen Canvas.
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
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BattleRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
