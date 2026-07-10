using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NodeSceneManager : MonoBehaviour
{
    private NodeSceneManager () { }
    public static NodeSceneManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SceneLoad(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void SceneUnLoad(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
}
