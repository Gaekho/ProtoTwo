using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBattleDirector : MonoBehaviour
{
    public static TempBattleDirector Instance { private set; get; }

    private TempBattleDirector() { }

    [SerializeField] private GameObject rewardPanel;

    public event Action OnBattleEnd;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    public void BattleWin()
    {
        rewardPanel.SetActive(true);
    }

    public void Continue()
    {
        SceneFlowManager.Instance.RequestBattleEnd();
    }
}
