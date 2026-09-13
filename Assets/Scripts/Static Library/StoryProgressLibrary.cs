using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StoryProgressLibrary
{

    public static bool ShouldStoryTrigger(int storyEventId)
    {
        return storyEventId == RuntimeState.Instance.GetCurrentStoryStep() + 1;
    }

    public static bool TryTriggerStoryEvent(int storyEventId, out string startPath)
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
