using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootUIController : MonoBehaviour
{
    #region Singleton
    public static RootUIController Instance { get; private set; } 
    private RootUIController() { }
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
