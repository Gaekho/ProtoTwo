using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllyUnitUIcontroller : BattleUnitUIcontroller
{
    [Header("Own Field \n =====================")]
    [Header("Stat Texts")]
    [SerializeField] private Text atk;
    [SerializeField] private Text shd;
    [SerializeField] private Text spd;

    [Header("Health")]
    [SerializeField] private float maxHealth;

    public override void SetUpUI(BattleUnitBase unit )
    {
        AllyUnit ally = unit as AllyUnit;
        maxHealth = ally.CharacterData.MaxHealth;
        //atk.text = ally.CurrentAttack.ToString();
        //shd.text = ally.CurrentShield.ToString();
        //spd.text = ally.CurrentSpeed.ToString();

        base.SetUpUI(unit);
    }

    public override void SetHealth(float currentHealth)
    {
        float sliderValue = currentHealth / maxHealth;
        healthSlider.value = sliderValue;
        healthTxt.text = $"{currentHealth} / {maxHealth}";
    }

    public void SetStatTexts(float attack, float shield, float speed)
    {
        atk.text = attack.ToString();
        shd.text = shield.ToString();
        spd.text = speed.ToString();
    }

}
