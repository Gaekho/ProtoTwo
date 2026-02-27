using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNode : NodeBase
{
    void Start()
    {
        nodeButton.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        Debug.Log("Boss Node Access");
    }
}
