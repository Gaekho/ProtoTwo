using Proto2.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BattleUnitUIcontroller : MonoBehaviour
{
    #region Field
    [Header("Owner")]
    [SerializeField] protected BattleUnitBase owner;
    [SerializeField] protected Canvas unitUICanvas;

    [Header("Turn Arrow")]
    [SerializeField] protected Image turnArrow;

    [Header("Health")]
    [SerializeField] protected Slider healthSlider;
    [SerializeField] protected Text healthTxt;

    [Header("Armor Text")]
    [SerializeField] protected Text armorTxt;

    [Header("Buff")]
    [SerializeField] protected Transform buffContainer;
    [SerializeField] protected GameObject buffUI;
    private readonly Dictionary<BuffTypes, BuffUI> buffUIs = new();

    #endregion

    public virtual void SetUpUI(BattleUnitBase unit)
    {
        turnArrow.gameObject.SetActive(false);
        SetHealth(owner.CurrentHealth);
        SetArmor(0);
    }

    public virtual void SetHealth(float currentHealth)
    {
        //하위 구현
    }

    public virtual void SetArmor(float amount)
    {
        armorTxt.text = amount.ToString();
    }

    public virtual void SetTurn(bool isTurn)
    {
        turnArrow.gameObject.SetActive(isTurn);
    }

    public virtual void CreateOrRefreshBuffUI(BuffInstance buff)
    {
        if (buff == null || buff.SourceBuff == null) return;

        BuffTypes type = buff.SourceBuff.BuffType;

        if (buffUIs.TryGetValue(type, out BuffUI existingUI))
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
        if (!buffUIs.TryGetValue(type, out BuffUI ui)) return;

        buffUIs.Remove(type);

        if (ui != null) Destroy(ui.gameObject);
    }

    public void RefreshAllBuffUI(List<BuffInstance> buffList)
    {
        foreach (BuffInstance buff in buffList)
        {
            CreateOrRefreshBuffUI(buff);
        }
    }
}
