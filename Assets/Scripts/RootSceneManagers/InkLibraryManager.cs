using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkLibraryManager : MonoBehaviour
{
    public static InkLibraryManager Instance { get;  private set; }
    private InkLibraryManager() { }
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

    [SerializeField] private TextAsset storyInk;
    [SerializeField] private TextAsset questInk;

    public TextAsset StoryInk => storyInk;
    public TextAsset QuestInk => questInk;
}
