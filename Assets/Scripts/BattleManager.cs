using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private BattleManager() { }
    public static BattleManager Instance {  get; private set; }
    private void Awake()
    {
        Instance = this;
        CardActionProcessor.Initialize();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
