using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    #region Singlton
    private BattleManager() { }
    public static BattleManager Instance {  get; private set; }
    #endregion

    #region Field
    [Header("Setting")]
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private HandController handContoller;

    [Header("Characters On Scene")]
    [SerializeField] private List<CharacterOnScene> playerParty;
    [SerializeField] private List<EnemyOnScene> enemyList;

    [Header("Turn")]
    [SerializeField] private int turn = 0;
    #endregion

    public IReadOnlyList<CharacterOnScene> PlayerParty => playerParty;
    private void Awake()
    {
        Instance = this;
        CardActionProcessor.Initialize();
        SetAlly();
        Debug.Log(PlayerParty[0].CharacterData.name);
        SetEnemy();
        HandController.Instance.SetUp();
    }

    private void SetAlly()
    {
        playerParty.Clear();
        GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally");

        foreach(GameObject go in allies)
        {
            playerParty.Add(go.GetComponentInChildren<CharacterOnScene>());
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
