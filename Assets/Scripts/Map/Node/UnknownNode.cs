using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnknownNode : NodeBase
{
    [Header("Incounter")]
    [SerializeField] private List<UnknownIncounter> incounters;

    public override void OnClick(string sceneName)
    {
        base.OnClick(sceneName);
        Debug.Log("Unknown Node Access");
    }

    private UnknownIncounter RandomIncounter()
    {
        int index = Random.Range(0, incounters.Count);
        return incounters[index];
    }
}
