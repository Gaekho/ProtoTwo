using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlowManager : MonoBehaviour
{
    #region Singleton
    private SceneFlowManager() { }
    public static SceneFlowManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public bool isTransitioning;
    
    // Only Way to Transition
    public void RequestNodeTransition(string targetNodeId)
    {
        if (isTransitioning) { }
        else
        {
            Debug.Log("Node Transition Start");
            TransitionNode(targetNodeId);
        }
    }
    private void TransitionNode(string targetNodeId)
    {
        isTransitioning = true;
        SceneManager.UnloadSceneAsync("NodeScene_" + RuntimeState.Instance.GetCurrentNodeId());
        SceneManager.LoadSceneAsync("NodeScene_" + targetNodeId, LoadSceneMode.Additive);
        RuntimeState.Instance.SetCurrentNode(targetNodeId);
        isTransitioning = false;
    }
}
