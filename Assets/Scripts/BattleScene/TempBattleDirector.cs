using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBattleDirector : MonoBehaviour
{
    [SerializeField] private GameObject rewardPanel;
    private void Awake()
    {
 
    }

    public void BattleWin()
    {
        rewardPanel.SetActive(true);
    }

    public void ContinueToNode()
    {
        SceneFlowManager.Instance.RequestBattleEnd();
    }
}
