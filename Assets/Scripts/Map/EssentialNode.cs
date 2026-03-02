using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu(fileName = "EssentialNode", menuName = "ScriptableObjects/EssentialNode")]
public class EssentialNode : ScriptableObject
{
    [Header("Node Type")]
    //필수로 넣을 방
    [SerializeField] private NodeType nodeType;
    //등장할 층. 미적용하고 싶을시 0으로 설정
    [SerializeField] private int floor;

    public NodeType GetNodeType() {  return nodeType; }
    public int GetFloor() { return floor; }
}
