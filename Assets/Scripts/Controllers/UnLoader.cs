using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnLoader : MonoBehaviour
{

    public void UnLoadScene()
    {
        NodeSceneManager.Instance.SceneUnLoad(gameObject.scene.name);
    }
}
