using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class RootUiManager : MonoBehaviour
{
    #region Singleton
    public static RootUiManager Instance { get; private set; } 
    private RootUiManager() { }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    #endregion

    [SerializeField] private Animator rootAnimator;


    // Binding with Unity Animation Clip Event : ScreenFadeOut & ScreenFadeIn
    public void FadeInDone() => GameEventsLibrary.RaiseFadeInDone();
    public void FadeOutDone() => GameEventsLibrary.RaiseFadeOutDone();
    public void TransitionAnimTrigger()
    {
        rootAnimator.SetTrigger("Transition");
    }

    // Trigger Methods
    public void FadeOutTrigger()
    {
        rootAnimator.SetTrigger("FadeOut");
    }
    public void FadeInTrigger()
    {
        rootAnimator.SetTrigger("FadeIn");
    }

}
