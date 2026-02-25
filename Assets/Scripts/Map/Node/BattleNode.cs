using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : NodeBase
{
    void Start()
    {
        nodeButton.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        Debug.Log("Battle Button Access");
        //차후 씬 연결 공부해서 여기에 넣을 예정
    }
}
