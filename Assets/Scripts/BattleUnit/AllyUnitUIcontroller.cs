using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllyUnitUIcontroller : MonoBehaviour
{
    //[SerializeField] private AllyUnit battleUnit;
    [Header("ThumbNail")]
    [SerializeField] private Image thumbNail;
    [SerializeField] private Image thumbNailColor;
    [SerializeField] private Outline outline;

    [Header("Stat Texts")]
    [SerializeField] private Text atk;
    [SerializeField] private Text shd;
    [SerializeField] private Text spd;

    [Header("Health")]
    [SerializeField] private float maxHealth;
    [SerializeField] private Text healthTxt;
    [SerializeField] private Slider hpSlider;

    [Header("Armor")]
    [SerializeField] private Text armorAmount;

    [Header("Buff")]
    [SerializeField] private GameObject buffUI;
    [SerializeField] private Transform buffContainer;
    public void SetUIC(AllyUnit unit )
    {
        outline.enabled = false;
        maxHealth = unit.CharacterData.MaxHealth;
        atk.text = unit.CurrentAttack.ToString();
        shd.text = unit.CurrentShield.ToString();
        spd.text = unit.CurrentSpeed.ToString();
        SetHealthSlider(unit.CurrentHealth);

        thumbNail.sprite = unit.CharacterData.ThumbNail;
        thumbNailColor.color = unit.CharacterData.UIColor;
        SetArmorAmount(0);
    }
    public void SetHealthSlider(float currentHealth)
    {
        hpSlider.value = currentHealth / maxHealth;
        healthTxt.text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }
    public void SetTurn(bool isTurn)
    {
        if(isTurn) outline.enabled = true;
        else outline.enabled = false;
    }

    public void SetArmorAmount(float amount)
    {
        armorAmount.text = amount.ToString();
    }

    public void CreateBuffUI( BuffInstance buff)
    {
            GameObject buffUIGO = Instantiate(buffUI, buffContainer);
            buffUIGO.GetComponent<BuffUI>().SetBuff(buff);
    }

    public void MergeBuffUI(BuffInstance original)
    {
    foreach (BuffUI buffUI in buffContainer.GetComponentsInChildren<BuffUI>())
        {
            if(buffUI.BuffType == original.SourceBuff.BuffType)
            {
                break;
            }
        }
    }
}
