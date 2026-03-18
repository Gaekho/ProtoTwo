using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NodeBase : MonoBehaviour
{
    [Header("Node Data")]
    [SerializeField] public NodeType nodetype;

    private int nodeIndex = -1;
    private bool bIsPlayerOn = false;
    private bool bIsConnected = false;
    private bool bIsActive = false;

    public List<NodeBase> nextNodes = new();
    public List<NodeBase> prevNodes = new();

    private Vector2 position = Vector2.zero;

    public NodeType GetNodeType() { return nodetype; }
    public int GetNodeIndex() { return nodeIndex; }
    public bool IsActivated() { return bIsActive; }
    public bool IsConnected() { return bIsConnected; }

    public Vector2 GetPosition() { return position; }
    

    public void SetNodeIndex(int newIndex) { nodeIndex = newIndex; }
    public void SetConnected() { bIsConnected = true; }
    public void SetActivate(bool inActivate) { bIsActive = inActivate; }

    public void SetUnActivate()
    {
        bIsPlayerOn = false;
        SetActivate(false);
    }

    public void SetPosition(int inX, int inY)
    {
        position.x = inX;
        position.y = inY;
    }

    public virtual void OnClick(string sceneName)
    {
        if(prevNodes.Count == 0)
        {
            Debug.Log("no prevNodes");
        }
        else
        {
            Debug.Log("prevNodes exist");
        }
        if (nextNodes.Count == 0)
        {
            Debug.Log("no nextNodes");
        }
        else
        {
            Debug.Log("nextNodes exist");
        }

        if (bIsActive)
        {
            foreach (NodeBase node in prevNodes)
            {
                node.SetUnActivate();
            }

            foreach(NodeBase node in nextNodes)
            {
                node.SetActivate(true);
            }

            bIsPlayerOn = true;
            SceneManager.LoadScene(sceneName);
        }

        //SceneManager.LoadScene(sceneName);
    }

    //private void Awake()
    //{
    //    if (nodeButton == null)
    //    {
    //        nodeButton = GetComponent<Button>();
    //    }
    //    Debug.Log("Node Generated");
    //}
}
