using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteBattleNode : NodeBase
{
    void Start()
    {
    }

    public override void OnClick(string sceneName)
    {
        base.OnClick(sceneName);
        Debug.Log("Elite Node Access");
    }
}
