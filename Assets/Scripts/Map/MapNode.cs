using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public enum NodeState
{
    None = 0,
    CurrentNode,
    Accessable,
    Disable
}
public class MapNode : MonoBehaviour
{
    [SerializeField] private string nodeId;
    [SerializeField] private NodeState state;


    // Refered by Pop-up's Move Button
    private void MoveScene()
    {
        NodeSceneManager.Instance.SceneLoad(nodeId);
    }
}
