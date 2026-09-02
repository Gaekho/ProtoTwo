using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryProgressManager : MonoBehaviour
{
    public static StoryProgressManager Instance;
    private StoryProgressManager() { }
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

    private Dictionary<string, int> currentValue = new();
    private Dictionary<string, int> lastShownValue = new();

    public void SetValue(string key, int value) => currentValue[key] = value;

    public bool ShouldStoryTrigger(string key)
    {
        int current = currentValue.TryGetValue(key, out var c) ? c : 0;
        int lastShown = lastShownValue.TryGetValue(key, out var i) ? 1 : -1;
        return current != lastShown;
    }

    public void MarkShown(string key)
    {
        lastShownValue[key] = currentValue.TryGetValue(key, out var c) ? c : 0;
    }

}
