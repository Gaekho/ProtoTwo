using System;
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
        string currentNodeId = RuntimeState.Instance.GetCurrentNodeId();

        if(currentNodeId == targetNodeId)
        {
            Debug.Log("Cannot Transition to Current Node");
            isTransitioning = false;
            yield break;
        }

        bool fadeOutDone = false;
        void HandleFadeOutDone() => fadeOutDone = true;
        RootUiManager.Instance.OnFadeOutDone += HandleFadeOutDone; 
        RootUiManager.Instance.FadeOutTrigger();
        yield return new WaitUntil(() => fadeOutDone);
        RootUiManager.Instance.OnFadeOutDone -= HandleFadeOutDone;

        Scene regionMap = SceneManager.GetSceneByName("RegionMap");
        if (regionMap.IsValid() && regionMap.isLoaded)
        {
            yield return SceneManager.UnloadSceneAsync(regionMap);
        }

        yield return SceneManager.UnloadSceneAsync("NodeScene_" + currentNodeId);
        yield return SceneManager.LoadSceneAsync("NodeScene_" + targetNodeId, LoadSceneMode.Additive);
        RuntimeState.Instance.SetCurrentNode(targetNodeId);

        bool fadeInDone = false;
        void HandleFadeInDone() => fadeInDone = true;
        RootUiManager.Instance.OnFadeInDone += HandleFadeInDone;
        RootUiManager.Instance.FadeInTrigger();
        yield return new WaitUntil(() => fadeInDone);
        RootUiManager.Instance.OnFadeInDone -= HandleFadeInDone;

        Debug.Log("Transition Finished");
        isTransitioning = false;
    }

    public IEnumerator RequestBattleEncounter()
    {
        yield return SceneManager.LoadSceneAsync("NewBattleScene", LoadSceneMode.Additive);
        yield return null;
    }

    public IEnumerator RequestSceneOverload(string targetScene)
    {
        yield return SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);
        yield return null;
    }
}
