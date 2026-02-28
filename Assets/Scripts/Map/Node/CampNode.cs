using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampNode : NodeBase
{
    void Start()
    {
    }

    public override void OnClick(string sceneName)
    {
        base.OnClick(sceneName);
        Debug.Log("Camp Node Access");
    }
}
