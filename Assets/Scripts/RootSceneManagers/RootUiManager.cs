using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
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
    public event Action OnFadeInDone;
    public event Action OnFadeOutDone;

    public void FadeInDone() => OnFadeInDone?.Invoke();
    public void FadeOutDone() => OnFadeOutDone?.Invoke();
    public void TransitionAnimTrigger()
    {
        rootAnimator.SetTrigger("Transition");
    }

    public void FadeOutTrigger()
    {
        rootAnimator.SetTrigger("FadeOut");
    }
    public void FadeInTrigger()
    {
        rootAnimator.SetTrigger("FadeIn");
    }

    public void GetCurrentState()
    {
        rootAnimator.GetCurrentAnimatorClipInfo(0);
    }
}
