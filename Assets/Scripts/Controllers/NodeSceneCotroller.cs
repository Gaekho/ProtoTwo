using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSceneCotroller : MonoBehaviour
{
    [SerializeField] string currentNode;
    // Start is called before the first frame update
    void Start()
    {
        if (StoryProgressManager.Instance.ShouldTrigger(currentNode))
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
