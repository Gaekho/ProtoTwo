using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class AllyStatUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [Header("Owner")]
    [SerializeField] private AllyUnit owner;

    [Header("Visual")]
    [SerializeField] private Image panel;
    [SerializeField] private Outline outLine;
    [SerializeField] private Image thumbnail;
    [SerializeField] private Color uiColor;

    [Header("Text List")]
    [SerializeField] private Text atkTxt;
    [SerializeField] private Text shdTxt;
    [SerializeField] private Text spdTxt;
    
    
    public void SetUp(AllyUnit ally)
    {
        panel.color = Color.white;
        


        // To Do : Set을 CharacterState로 변경하는 방안 + 탈리스만 확보해서 개수 표시.
    }



    public void OnPointerEnter(PointerEventData data)
    {
        if (data.pointerDrag)
        {

        }
    }

    public void OnPointerExit(PointerEventData data)
    {

    }
}
