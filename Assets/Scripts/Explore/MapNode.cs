using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Proto2.Enums;
using TMPro;

public class MapNode : MonoBehaviour
{
    #region Field
    [Header("Own Node ID")]
    [SerializeField] private string ownNodeId;
    public string OwnNodeId => ownNodeId;

    [Header("Visual Elements")]
    [SerializeField] private Image background;
    [SerializeField] private Image nodeIcon;
    [SerializeField] private Image visitMask;
    [SerializeField] private Image accessable;
    [SerializeField] private Image encounterSticker;
    [SerializeField] private TMP_Text nodeName;
    [SerializeField] private GameObject popUp;
    [SerializeField] private Button goButton;
    private Color orange = new (1, 0.7f, 0, 1);
    #endregion
    
    /* Color Sheet of NodeState

    Backgroun(DangerLevel) : 
    Safe  | Caution | Danger
    ------|---------|-------
    green | yellow  | red

    Visit Mask :
    Visited | Non-Visited 
    --------|-------------
    clear   |(1, 1, 1, 0.4)

    Accessable : 
    Current | Accessable | Disable
    --------|------------|---------
    orange  |    cyan    | clear

     */
    private void Start()
    {
        UpdateVisual(RuntimeState.Instance.GetNodeState(ownNodeId));
    }
    // Update Visual
    public void UpdateVisual(NodeState nodeState)
    {
        // Set Background Color
        switch (nodeState.DangerLevel)
        {
            case (NodeDangerLevel.Safe): background.color = Color.green; break;

            case(NodeDangerLevel.Caution): background.color = Color.yellow; break;

            case (NodeDangerLevel.Danger): background.color = Color.red; break;
        }

        // Set Visit Mask
        if (nodeState.Visited)
        {
            visitMask.color = Color.clear;
        }
        else visitMask.color = new(1, 1, 1, 0.4f);

        // Set Accessable
        switch (nodeState.Accessable)
        {
            case (NodeAccessable.CurrentNode):accessable.color = orange; break;

            case (NodeAccessable.Accessable): accessable.color = Color.cyan; break;

            case (NodeAccessable.Disable): accessable.color = Color.clear; break;
        }

        // Set Encounter
        if (nodeState.Encounter)
        {
            encounterSticker.gameObject.SetActive(true);
        }
        else encounterSticker.gameObject.SetActive(false);
        
    }
    // Update Pop-up
    public void UpdatePopUp()
    {
        if (RuntimeState.Instance.GetCurrentNodeId() == ownNodeId) 
        {
            goButton.interactable = false;
        }
    }
    // Refered by Click Node
    public void ShowPopUp()
    {
        popUp.SetActive(true);
        UpdatePopUp();
    }
    // Refered by Pop-up's Move Button
    public void MoveScene()
    {
        SceneFlowManager.Instance.RequestNodeTransition(ownNodeId);
    }
}
