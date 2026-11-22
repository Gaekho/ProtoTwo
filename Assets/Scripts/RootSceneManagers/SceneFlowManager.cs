using Proto2.Enums;
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
    private EncounterData pendingEncounter;

    #region Public Request Methods
    public void RequestNodeTransition(string targetNodeId)
    {
        if (isTransitioning) { }
        else
        {
            Debug.Log("Node Transition Start");
            StartCoroutine(NodeTransitionRoutine(targetNodeId));
        }
    }

    public void RequestBattleEncounter(EncounterData encounter)
    {
        pendingEncounter = encounter;
        StartCoroutine(BattleEncounterTransition());
    }

    public void RequestBattleEnd()
    {
        StartCoroutine(BattleEndTransition());
    }

    public void RequestSceneOverload(string targetScene)
    {
        StartCoroutine(SceneOverloadRoutine(targetScene));
    }

    public void RequestSceneUnload(string targetScene)
    {
        StartCoroutine(SceneUnloadRoutine(targetScene));
    }

    #endregion

    #region Private Routines
    private IEnumerator NodeTransitionRoutine(string targetNodeId)
    {
        isTransitioning = true;
        string currentNodeId = RuntimeState.Instance.GetCurrentNodeId();

        if(currentNodeId == targetNodeId)
        {
            Debug.Log("Cannot Transition to Current Node");
            isTransitioning = false;
            yield break;
        }

        yield return PlayFadeOut();

        RuntimeState.Instance.GetNodeState(currentNodeId).SetAccessable(NodeAccessable.Accessable);

        Scene regionMap = SceneManager.GetSceneByName("RegionMap");
        if (regionMap.IsValid() && regionMap.isLoaded)
        {
            yield return SceneManager.UnloadSceneAsync(regionMap);
        }

        yield return SceneManager.UnloadSceneAsync("NodeScene_" + currentNodeId);
        yield return SceneManager.LoadSceneAsync("NodeScene_" + targetNodeId, LoadSceneMode.Additive);
        RuntimeState.Instance.SetCurrentNodeId(targetNodeId);

        yield return PlayFadeIn();

        Debug.Log("Transition Finished");
        isTransitioning = false;
    }

    private IEnumerator BattleEncounterTransition()
    {
        yield return SceneManager.UnloadSceneAsync("ExploreBaseScene");
        yield return SceneManager.LoadSceneAsync("NewBattleScene", LoadSceneMode.Additive);
        yield return null;
    }

    private IEnumerator BattleEndTransition()
    {
        yield return PlayFadeOut();

        yield return SceneManager.UnloadSceneAsync("NewBattleScene");
        yield return SceneManager.LoadSceneAsync("ExploreBaseScene", LoadSceneMode.Additive);

        Debug.Log("Before BattleEnd Event");
        GameEventsLibrary.RaiseBattleEnd();

        yield return PlayFadeIn();

    }

    private IEnumerator SceneOverloadRoutine(string targetScene)
    {
        yield return SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);
        yield return null;
    }

    private IEnumerator SceneUnloadRoutine(string targetScene)
    {
        yield return SceneManager.UnloadSceneAsync(targetScene);
    }
    #endregion

    #region Utilities
    private IEnumerator PlayFadeOut()
    {
        bool done = false;
        void Handler() => done = true;
        GameEventsLibrary.OnFadeOutDone += Handler;
        RootUiManager.Instance.FadeOutTrigger();
        yield return new WaitUntil(() => done);
        GameEventsLibrary.OnFadeOutDone -= Handler;
    }

    private IEnumerator PlayFadeIn()
    {
        bool done = false;
        void Handler() => done = true;
        GameEventsLibrary.OnFadeInDone+= Handler;
        RootUiManager.Instance.FadeInTrigger();
        yield return new WaitUntil(() => done);
        GameEventsLibrary.OnFadeInDone -= Handler;
    }

    public EncounterData ConsumePendingEncounter()
    {
        EncounterData data = pendingEncounter;
        pendingEncounter = null;
        return data;
    }
    #endregion
}
