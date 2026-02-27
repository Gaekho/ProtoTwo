using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnknownNode : NodeBase
{
    // Start is called before the first frame update
    void Start()
    {
        nodeButton.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        Debug.Log("Unknown Node Access");
    }
}
