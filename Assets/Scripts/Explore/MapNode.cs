using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public enum NodeAccessable
{
    None = 0,
    CurrentNode,
    Accessable,
    Disable
}
public class MapNode : MonoBehaviour
{
    [SerializeField] private string targetNodeId;
    [SerializeField] private NodeState state;
    [SerializeField] private NodeAccessable accessable;
    [SerializeField] private GameObject popUp;
    [SerializeField] private Button goButton;
    // Update Pop-up
    public void UpdatePopUp()
    {
        if (RuntimeState.Instance.GetCurrentNodeId() == targetNodeId) 
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
        SceneFlowManager.Instance.RequestNodeTransition(targetNodeId);
    }
}
