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

    public bool ShouldStoryTrigger(int storyEventId)
    {
        return storyEventId == RuntimeState.Instance.GetCurrentStoryStep() + 1;
    }

    public bool TryTriggerStoryEvent(int storyEventId, out string startPath)
    {
        if(ShouldStoryTrigger(storyEventId) == false)
        {
            startPath = null;
            return false;
        }
        RuntimeState.Instance.SetCurrentStoryStep(storyEventId);
        startPath = InkPathProvider.GetStoryKnot(storyEventId);
        return true;
    }
}
