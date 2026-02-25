using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftNode : NodeBase
{
    void Start()
    {
        nodeButton.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        Debug.Log("Gift Node Access");
    }
}
