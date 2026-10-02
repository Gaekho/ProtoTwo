using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using TMPro;
using Proto2.Enums;
using System.Runtime.CompilerServices;
using System;

public class BattleUiController : MonoBehaviour
{
    #region Field
    [Header("Turn Change Panel")]
    [SerializeField] private GameObject turnChangePanel;
    [SerializeField] private TMP_Text whosTurn;
    [SerializeField] private Text turnCount;

    [SerializeField] private Canvas screenCanvas;

    [Header("Reward Panel")]
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private TMP_Text winOrLose;
    [SerializeField] private RectTransform RewardContainer;

    [Header("Turn Queue")]
    [SerializeField] private TurnQUIController qUIController;

    [Header("Ally Stats")]
    [SerializeField] private AllyStatPanel sitaPanel;
    [SerializeField] private AllyStatPanel solphurPanel;
    [SerializeField] private AllyStatPanel rigelPanel;

    [Serializable]
    private class AllyStatPanel
    {
        [SerializeField] private Image outline;
        [SerializeField] private Text atk, shd, spd;

        public void SetStat(int atk, int shd, int spd)
        {
            outline.color = Color.white;
            this.atk.text = $"{atk}";
            this.shd.text = $"{shd}";
            this.spd.text = $"{spd}";
        }
        public void UpdateStat(int atk, int shd, int spd)
        {
            this.atk.text = $"{atk}";
            this.shd.text = $"{shd}";
            this.spd.text = $"{spd}";
        }
        public void EnterTurn()
        {
            outline.color = Color.white;
        }
        public void ExitTurn()
        {
            outline.color = Color.white;
        }
    }
    #endregion

     
    // Start is called before the first frame update

    private void Awake()
    {
        turnChangePanel.SetActive(false);
        rewardPanel.SetActive(false);
    }

    // NotUsed
    //public IEnumerator TurnStart(int turn, string who)
    //{
    //    turnChangePanel.SetActive(true);
    //    TMP_Text whosTurn = turnChangePanel.GetComponentInChildren<TMP_Text>();
    //    Text turnCount = turnChangePanel.GetComponentInChildren<Text>();

    //    whosTurn.text = who + " Turn";
    //    turnCount.text = turn.ToString();
    //    yield return new WaitForSeconds(1.5f);
    //    turnChangePanel.SetActive(false);
    //    yield break;
    //}

    //public IEnumerator TurnEnd()
    //{
    //    characterSelectPanel.SetActive(true);

    //    while (BattleManager.Instance.CurrentState == TurnState.End)
    //    {
    //        yield return null;
    //    }
    //    //yield return new WaitForSeconds(0.5f);
    //    characterSelectPanel.SetActive(false);
    //    yield break;
    //}

    public IEnumerator UnitTurnStart(int turn, string who)
    {
        turnChangePanel.SetActive(true);
        //TMP_Text whosTurn = turnChangePanel.GetComponentInChildren<TMP_Text>();
        //Text turnCount = turnChangePanel.GetComponentInChildren<Text>();

        whosTurn.text = who + "의 차례";
        turnCount.text = turn.ToString();

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
        TMP_Text wl = rewardPanel.GetComponentInChildren<TMP_Text>();
        wl.text = "전투 " + winLose + "!";
        yield break;
    }

    public IEnumerator RoundStart(int queueCount)
    {
        turnChangePanel.SetActive(true);

        TMP_Text whosTurn = turnChangePanel.GetComponentInChildren<TMP_Text>();
        Text turnCount = turnChangePanel.GetComponentInChildren<Text>();
        qUIController.transform.localScale = new Vector3(1.5f, 1.5f);

        whosTurn.text = $"{queueCount} 번째 라운드 진행";
        turnCount.text = "";

        yield return new WaitForSeconds(1.5f);
        qUIController.transform.localScale = Vector3.one;
        turnChangePanel.SetActive(false);
    }

    public void RefreshTurnQueueUI(int queueCount, List<BattleUnitBase> aliveUnits)
    {
        qUIController.ReBuildQueue(queueCount, aliveUnits);
    }

    public void TransferTurnQueueUI()
    {
        qUIController.TransferQueueUI();
    }

    public void RemoveUnitFromTurnQueueUI(BattleUnitBase deadUnit)
    {
        qUIController.RemoveUnitFromQueueUI(deadUnit);
    }


    #region Stat Panel
    public void SetStatPanel()
    {

        
    }

    public void UpdateStatUI(AllyUnit unit, int atk, int shd, int spd)
    {
        switch (unit.CharacterData.CardColor)
        {
            case CardColor.Red:
                sitaPanel.UpdateStat(atk, shd, spd); break;

            case CardColor.Green:
                solphurPanel.UpdateStat(atk, shd, spd); break;

            case CardColor.Blue:
                rigelPanel.UpdateStat(atk, shd, spd); break;
        }

    }
    #endregion
}
