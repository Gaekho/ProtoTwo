using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnknownNode : NodeBase
{
    // Start is called before the first frame update
    void Start()
    {
    }

    public override void OnClick(string sceneName)
    {
        Debug.Log("Unknown Node Access");
    }
}
