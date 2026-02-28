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

    private List<NodeBase> connectedNode = new List<NodeBase>();
    private int nodeIndex = -1;
    private bool bIsPlayerOn = false;
    private bool bIsConnected = false;

    public NodeType GetNodeType() { return nodetype; }
    public int GetNodeIndex() { return nodeIndex; }
    public void SetNodeIndex(int newIndex) { nodeIndex = newIndex; }
    public void ConnectNode(NodeBase node) {  connectedNode.Add(node); }
    public void SetActivate() { bIsConnected = true; }

    public virtual void OnClick(string sceneName)
    {
        if (bIsConnected)
        {
            bIsPlayerOn = true;
            SceneManager.LoadScene(sceneName);
        }
    }

    void Update()
    {
        if(bIsPlayerOn)
        {
            foreach (NodeBase node in connectedNode)
            {
                node.SetActivate();
            }
        }
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
