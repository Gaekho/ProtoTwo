using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNode : NodeBase
{
    void Start()
    {
    }

    public override void OnClick(string sceneName)
    {
        Debug.Log("Boss Node Access");
    }
}
