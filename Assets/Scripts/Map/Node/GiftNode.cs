using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftNode : NodeBase
{
    [Header("Incounter")]
    [SerializeField] private List<GiftIncounter> incounters;

    public override void OnClick(string sceneName)
    {
        base.OnClick(sceneName);
        Debug.Log("Gift Node Access");
    }

    private GiftIncounter RandomIncounter()
    {
        int index = Random.Range(0, incounters.Count);
        return incounters[index];
    }
}
