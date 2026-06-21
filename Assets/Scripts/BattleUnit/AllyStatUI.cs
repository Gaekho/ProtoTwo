using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllyStatUI : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private Outline outLine;
    [SerializeField] private Image outLineImage;
    [SerializeField] private Image thumbnail;
    [SerializeField] private AllyUnit owner;

    [Header("Text List")]
    [SerializeField] private Text atkTxt;
    [SerializeField] private Text shdTxt;
    [SerializeField] private Text spdTxt;
    
    
    public void SetUp()
    {
        if(owner.CharacterData == null) { Debug.Log("null"); }
        outLine.enabled = false;
        outLineImage.color = owner.CharacterData.UIColor;
        thumbnail.sprite = owner.CharacterData.ThumbNail;
        SetStatText(owner.CharacterData.BaseAttack, owner.CharacterData.BaseShield, owner.CharacterData.BaseSpeed);
    }

    public void SetStatText(float atk, float shd, float spd)
    {
        atkTxt.text = atk.ToString();
        shdTxt.text = shd.ToString();
        spdTxt.text = spd.ToString();
    }

    public void EnterTurn()
    {
        outLine.enabled = true;
        gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1);
    }

    public void ExitTurn()
    {
        outLine.enabled = false;
        gameObject.transform.localScale = Vector3.one;

    }

    public Color GetColor()
    {
        return outLineImage.color;
    }

}
