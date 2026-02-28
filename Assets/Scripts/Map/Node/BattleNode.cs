using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleNode : NodeBase
{
    void Start()
    {
    }

    public override void OnClick(string sceneName)
    {
        base.OnClick(sceneName);
        Debug.Log("Battle Button Access");
    }
}
