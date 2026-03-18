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
    [SerializeField] public Sprite buttonImage;
    [SerializeField] public Button nodeButton;

    private int nodeIndex = -1;
    private bool bIsPlayerOn = false;
    private bool bIsConnected = false;
    private bool bIsActive = false;

    public List<NodeBase> nextNodes;
    public List<NodeBase> prevNodes;

    private Vector2 position = Vector2.zero;

    public NodeType GetNodeType() { return nodetype; }
    public int GetNodeIndex() { return nodeIndex; }
    public bool IsActivated() { return bIsActive; }
    public bool IsConnected() { return bIsConnected; }

    public Vector2 GetPosition() { return position; }
    

    public void SetNodeIndex(int newIndex) { nodeIndex = newIndex; }
    public void SetConnected() { bIsConnected = true; }
    public void SetActivate() { bIsActive = true; }

    public void SetPosition(int inX, int inY)
    {
        position.x = inX;
        position.y = inY;
    }

    public virtual void OnClick(string sceneName)
    {
        if (bIsActive)
        {
            bIsPlayerOn = true;
            SceneManager.LoadScene(sceneName);
        }

        SceneManager.LoadScene(sceneName);
    }

    private void Awake()
    {
        if (nodeButton == null)
        {
            nodeButton = GetComponent<Button>();
        }
        Debug.Log("Node Generated");
    }
}
