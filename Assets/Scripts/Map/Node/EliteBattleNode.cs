using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteBattleNode : NodeBase
{
    void Start()
    {
        nodeButton.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        Debug.Log("Elite Node Access");
    }
}
