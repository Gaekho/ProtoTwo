using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIController : BattleUnitUIcontroller
{
    [Header("=============================== \n Own Field")]
    //[SerializeField] private EnemyUnit owner;
    [SerializeField] private Canvas myCanvas;
    [SerializeField] private Text armorText;
    [SerializeField] private Image intentIcon;
    [SerializeField] private float maxHealth;

    public override  void SetUpUI(BattleUnitBase unit)
    {
        EnemyUnit enemy = unit as EnemyUnit;
        maxHealth = enemy.EnemyData.MaxHealth;
        intentIcon.sprite = null;

        base.SetUpUI(unit);

    }

    public override void SetHealth(float currentHealth)
    {
        healthSlider.value = currentHealth / maxHealth;
        healthTxt.text = $"{currentHealth.ToString()} / {maxHealth.ToString()}";
    }
    public void SetArmorText(float value)
    {
        armorText.text = value.ToString();
    }

    public void SetPatternImage(EnemyPatternData data)
    {
        if (data == null || data.IntentIcon == null) return;

        intentIcon.sprite = data.IntentIcon;
    }

}
