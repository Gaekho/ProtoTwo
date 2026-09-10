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
        //RootUiManager.Instance.OnFadeOutDone += HandleFadeOutDone;
        GameEvents.OnFadeOutDone += HandleFadeOutDone;
        RootUiManager.Instance.FadeOutTrigger();
        yield return new WaitUntil(() => fadeOutDone);
        //RootUiManager.Instance.OnFadeOutDone -= HandleFadeOutDone;
        GameEvents.OnFadeOutDone -= HandleFadeOutDone;

        Scene regionMap = SceneManager.GetSceneByName("RegionMap");
        if (regionMap.IsValid() && regionMap.isLoaded)
        {
            yield return SceneManager.UnloadSceneAsync(regionMap);
        }

        yield return SceneManager.UnloadSceneAsync("NodeScene_" + currentNodeId);
        yield return SceneManager.LoadSceneAsync("NodeScene_" + targetNodeId, LoadSceneMode.Additive);
        RuntimeState.Instance.SetCurrentNodeId(targetNodeId);

        bool fadeInDone = false;
        void HandleFadeInDone() => fadeInDone = true;
        //RootUiManager.Instance.OnFadeInDone += HandleFadeInDone;
        GameEvents.OnFadeInDone += HandleFadeInDone;
        RootUiManager.Instance.FadeInTrigger();
        yield return new WaitUntil(() => fadeInDone);
        //RootUiManager.Instance.OnFadeInDone -= HandleFadeInDone;
        GameEvents.OnFadeInDone -= HandleFadeInDone;

        Debug.Log("Transition Finished");
        isTransitioning = false;
    }

    public void RequestBattleEncounter()
    {
        StartCoroutine(BattleEncounterTransition());
    }

    public IEnumerator BattleEncounterTransition()
    {
        yield return SceneManager.UnloadSceneAsync("ExploreBaseScene");
        yield return SceneManager.LoadSceneAsync("NewBattleScene", LoadSceneMode.Additive);
        yield return null;
    }
    public void RequestBattleEnd()
    {
        StartCoroutine(BattleEndTransition());
    }

    public IEnumerator BattleEndTransition()
    {
        bool fadeOutDone = false;
        void HandleFadeOutDone() => fadeOutDone = true;
        GameEvents.OnFadeOutDone += HandleFadeOutDone;
        RootUiManager.Instance.FadeOutTrigger();
        yield return new WaitUntil(() => fadeOutDone);
        GameEvents.OnFadeOutDone -= HandleFadeOutDone;

        yield return SceneManager.UnloadSceneAsync("NewBattleScene");
        yield return SceneManager.LoadSceneAsync("ExploreBaseScene", LoadSceneMode.Additive);

        Debug.Log("Before BattleEnd Event");
        GameEvents.RaiseBattleEnd();

        bool fadeInDone = false;
        void HandleFadeInDone() => fadeInDone = true;
        GameEvents.OnFadeInDone += HandleFadeInDone;
        RootUiManager.Instance.FadeInTrigger();
        yield return new WaitUntil(() => fadeInDone);
        GameEvents.OnFadeInDone -= HandleFadeInDone;
        yield return null;
    }

    public IEnumerator RequestSceneOverload(string targetScene)
    {
        yield return SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);
        yield return null;
    }
}
