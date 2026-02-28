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

    public NodeType GetNodeType() { return nodetype; }

    public virtual void OnClick(string sceneName)
    {
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
