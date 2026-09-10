using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeState
{
    public bool Visited {  get; private set; }
    public NodeDangerLevel DangerLevel {  get; private set; }
    public bool Encounter {  get; private set; }
    public NodeAccessable Accessable { get; private set; }

    private Dictionary<string, int> interactionCounts;      // <Interaction ID(or Name), visited Count>

    // Basic Creator
    public NodeState()
    {
        Visited = false;
        DangerLevel = NodeDangerLevel.Safe;
        Encounter = false;
        interactionCounts = new();
    }

    public void VisitNode()
    {
        Visited = true;
    }

    public void SetDangerLevel(NodeDangerLevel level)
    {
        DangerLevel = level;
    }

    public void SetEncounter(bool encounter)
    {
        Encounter = encounter;
    }

    public int GetInteractionCount(string interactionKey)
    {
        return interactionCounts.TryGetValue(interactionKey, out var count) ? count : 0;
    }

    public bool HasInteracted(string interactionKey)
    {
        return GetInteractionCount(interactionKey) > 0;
    }

    public void IncreaseInteraction(string interactionKey)
    {
        interactionCounts.TryGetValue(interactionKey, out var count);
        interactionCounts[interactionKey] = count + 1;
    }
}
