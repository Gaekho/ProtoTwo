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
    #endregion
    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;
        turnChangePanel.SetActive(false);
        characterSelectPanel.SetActive(false);
    }

    public IEnumerator TurnStart(int turn, string who)
    {
        turnChangePanel.SetActive(true);
        TMP_Text whosTurn = turnChangePanel.GetComponentInChildren<TMP_Text>();
        Text turnCount = turnChangePanel.GetComponentInChildren<Text>();

        whosTurn.text = who + " Turn";
        turnCount.text = turn.ToString();
        yield return new WaitForSeconds(1f);
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
