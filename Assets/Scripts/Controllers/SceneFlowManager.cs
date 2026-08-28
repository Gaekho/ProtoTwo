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
            StartCoroutine(TransitionNode(targetNodeId));
        }
    }
    private IEnumerator TransitionNode(string targetNodeId)
    {
        isTransitioning = true;
        RootUIController.Instance.FadeOutTrigger();
        Scene regionMap = SceneManager.GetSceneByName("RegionMap");
        if(regionMap.IsValid() && regionMap.isLoaded)
        {
            yield return SceneManager.UnloadSceneAsync(regionMap);
        }
        yield return SceneManager.UnloadSceneAsync("NodeScene_" + RuntimeState.Instance.GetCurrentNodeId());
        yield return SceneManager.LoadSceneAsync("NodeScene_" + targetNodeId, LoadSceneMode.Additive);
        RuntimeState.Instance.SetCurrentNode(targetNodeId);
        RootUIController.Instance.FadeInTrigger();
        isTransitioning = false;
    }
}
