using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnLoader : MonoBehaviour
{
    public void LoadAdditiveScene(string sceneName)
    {
        NodeSceneManager.Instance.SceneLoad(sceneName);
    }
    public void UnLoadThisScene()
    {
        NodeSceneManager.Instance.SceneUnLoad(gameObject.scene.name);
    }

    public void ToAnotherMap(string mapName)
    {

    }
}
