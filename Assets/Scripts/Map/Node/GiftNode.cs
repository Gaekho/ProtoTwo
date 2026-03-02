using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftNode : NodeBase
{
    void Start()
    {
    }

    public override void OnClick(string sceneName)
    {
        base.OnClick(sceneName);
        Debug.Log("Gift Node Access");
    }
}
