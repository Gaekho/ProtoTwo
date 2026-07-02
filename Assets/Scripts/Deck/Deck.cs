using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

//json으로 저장할 데이터
[Serializable]
public class DeckSaveData
{
    string deckName;
    DeckData deckData;
}
