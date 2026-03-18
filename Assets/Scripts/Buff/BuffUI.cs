using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffUI : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private BuffTypes buffType;
    [SerializeField] private BuffInstance currentBuff;

    [Header("UI")]
    [SerializeField] private Image buffSprite;
    [SerializeField] private Text buffDuration;

    [SerializeField] private int remainTurn;

    [Header("Icon Database")]
    [SerializeField] private BuffIconEntry [] iconEntry;

    public BuffTypes BuffType => buffType;

    public void SetBuff(BuffInstance buffInstance)
    {
        currentBuff = buffInstance;
        buffType = buffInstance.SourceBuff.BuffType;

        Refresh();
    }

    public void Refresh()
    {
        if(currentBuff == null || currentBuff.SourceBuff == null) return;

        buffType = currentBuff.SourceBuff.BuffType;

        if(currentBuff.SourceBuff.ReduceTiming == ReduceTiming.Permanent)   
            buffDuration.text = "¡Ä";

        else 
            buffDuration.text = currentBuff.RemainTurn.ToString();

        Sprite icon = GetIcon(buffType);
        if(icon != null)
        {
            buffSprite.sprite = icon;
        }
    }

    public void MergeToSelf(BuffInstance buffInstance)
    {
        remainTurn += buffInstance.RemainTurn;
        buffDuration.text = remainTurn.ToString();
    }

    public void DurationDown()
    {

    }

    public void BuffEnd()
    {
        Destroy(gameObject);
    }

    private Sprite GetIcon(BuffTypes type)
    {
        for(int i = 0; i < iconEntry.Length; i++)
        {
            if (iconEntry[i].buffType == type)
                return iconEntry[i].icon;
        }
        return null;
    }
}

[System.Serializable]
public struct BuffIconEntry
{
    public BuffTypes buffType;
    public Sprite icon;
}