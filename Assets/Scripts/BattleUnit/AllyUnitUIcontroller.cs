using Proto2.Enums;
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

    private readonly Dictionary<BuffTypes, BuffUI> buffUIs = new();

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
        outline.enabled = isTurn;
    }

    public void SetArmorAmount(float amount)
    {
        armorAmount.text = amount.ToString();
    }

    public void SetStatTexts(float attack, float shield, float speed)
    {
        atk.text = attack.ToString();
        shd.text = shield.ToString();
        spd.text = speed.ToString();
    }

    public void CreateOrRefreshBuffUI(BuffInstance buff)
    {
        if(buff == null || buff.SourceBuff == null) return;

        BuffTypes type = buff.SourceBuff.BuffType;

        if(buffUIs.TryGetValue(type, out BuffUI existingUI))
        {
            existingUI.SetBuff(buff);
            return;
        }

        GameObject buffUIGO = Instantiate(buffUI, buffContainer);
        buffUIGO.GetComponent<BuffUI>().SetBuff(buff);

        buffUIs[type] = buffUIGO.GetComponent<BuffUI>();
    }

    public void RemoveBuffUI(BuffTypes type)
    {
        if(!buffUIs.TryGetValue(type, out BuffUI ui)) return;

        buffUIs.Remove(type);

        if (ui != null) Destroy(ui.gameObject);
    }

    public void RefreshAllBuffUI(List<BuffInstance> buffList)
    {
        foreach(BuffInstance buff in buffList)
        {
            CreateOrRefreshBuffUI(buff);
        }
    }

}
