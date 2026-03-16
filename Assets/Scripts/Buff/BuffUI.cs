using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffUI : MonoBehaviour
{
    [SerializeField] private BuffTypes buffType;
    [SerializeField] private Image buffSprite;
    [SerializeField] private Text buffDuration;
    [SerializeField] private int remainTurn;
    [SerializeField] private List<Sprite> iconList;

    public BuffTypes BuffType => buffType;

    public void SetBuff(BuffInstance buffInstance)
    {
        buffType = buffInstance.SourceBuff.BuffType;
        remainTurn = buffInstance.RemainTurn;
        buffDuration.text = remainTurn.ToString();
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
}
