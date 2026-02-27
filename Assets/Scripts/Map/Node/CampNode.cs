using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampNode : NodeBase
{
    void Start()
    {
        nodeButton.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        Debug.Log("Camp Node Access");
    }
}
