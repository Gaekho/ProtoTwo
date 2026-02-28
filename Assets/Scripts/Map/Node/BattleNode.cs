using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : NodeBase
{
    void Start()
    {
    }

    public void OnClick(string sceneName)
    {
        Debug.Log("Battle Button Access");
       
        SceneManager.LoadScene(sceneName);
    }
}
