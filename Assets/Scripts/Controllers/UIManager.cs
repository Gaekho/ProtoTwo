using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using Proto2.Enums;

public class UIManager : MonoBehaviour
{
    #region Singleton
    private UIManager() { }
    public static UIManager Instance { get; private set; }
    #endregion

    #region Field
    [SerializeField] private Canvas screenCanvas;
    [SerializeField] private GameObject turnChangePanel;
    [SerializeField] private GameObject characterSelectPanel;
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private TurnQUIController qUIController;
    #endregion
    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;
        turnChangePanel.SetActive(false);
        characterSelectPanel.SetActive(false);
        rewardPanel.SetActive(false);
    }

    public IEnumerator TurnStart(int turn, string who)
    {
        turnChangePanel.SetActive(true);
        TMP_Text whosTurn = turnChangePanel.GetComponentInChildren<TMP_Text>();
        Text turnCount = turnChangePanel.GetComponentInChildren<Text>();

        whosTurn.text = who + " Turn";
        turnCount.text = turn.ToString();
        yield return new WaitForSeconds(1.5f);
        turnChangePanel.SetActive(false);
        yield break;
    }

    public IEnumerator TurnEnd()
    {
        characterSelectPanel.SetActive(true);

        while (BattleManager.Instance.CurrentState == TurnState.End)
        {
            yield return null;
        }
        //yield return new WaitForSeconds(0.5f);
        characterSelectPanel.SetActive(false);
        yield break;
    }

    public IEnumerator UnitTurnStart(int turn, string who)
    {
        turnChangePanel.SetActive(true);
        TMP_Text whosTurn = turnChangePanel.GetComponentInChildren<TMP_Text>();
        Text turnCount = turnChangePanel.GetComponentInChildren<Text>();

        whosTurn.text = who + "'s Turn";
        turnCount.text = turn.ToString();

        qUIController.OnUnitTurnStart();

        yield return new WaitForSeconds(0.8f);
        turnChangePanel.SetActive(false);
        yield break;
    }
    public IEnumerator UnitTurnEnd()
    {
        yield break;
    }

    public IEnumerator BattleEnd(string winLose)
    {
        rewardPanel.SetActive(true);
        TMP_Text wl = GetComponentInChildren<TMP_Text>();
        wl.text = "전투 " + winLose + "!";
        yield break;
    }

    public void RefreshTurnQueue(int queueCount, List<BattleUnitBase> aliveUnits)
    {
        qUIController.ReBuildQueue(queueCount, aliveUnits);
    }

    public void Selection()
    {

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
