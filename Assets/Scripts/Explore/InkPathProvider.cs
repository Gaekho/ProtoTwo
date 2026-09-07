using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InkPathProvider
{
    public static string GetStoryKnot(int storyEventId) => $"story_{storyEventId:D3}";

    public static string GetNodeInteractionKnot(string nodeId, string interactionKey) => $"node_{nodeId}_{interactionKey}";

    public static string GetQuestKnot(int questId, string phase) => $"quest_{questId:D4}_{phase}";
}

