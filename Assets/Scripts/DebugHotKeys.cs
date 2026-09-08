using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHotKeys : MonoBehaviour
{      
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.F1))
        {
                RuntimeState.Instance.GetNodeState("0001").SetEncounter(true);
                Debug.Log("0001 노드에 인카운터 강제 설정 완료");
            }
    }
}