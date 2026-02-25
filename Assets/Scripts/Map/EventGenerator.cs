using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGenerator : MonoBehaviour
{
    [Header("Level Data")]
    //รั รþผ๖
    [SerializeField] private int floors = 10;
    [SerializeField] private int minRoom = 1;
    [SerializeField] private int maxRoom = 4;
    [SerializeField] private EssentialNode essential;



    public void GenerateMap()
    {

    }

    public void GenerateFloor(int currentFloor)
    {
        if(currentFloor == essential.GetFloor())
        {

        }
    }

    public void GenerateNode(NodeType nodeType)
    {

    }
}
