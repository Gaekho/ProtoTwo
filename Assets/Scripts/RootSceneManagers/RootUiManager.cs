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
}
