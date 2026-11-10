using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllyUnitUIcontroller : BattleUnitUIcontroller
{
    [Header("Own Field \n =====================")]

    [Header("Health")]
    [SerializeField] private int maxHealth;

    [Header("Gnosis")]
    [SerializeField] private Color originalColor = new (0.245283f, 0.245283f, 0.245283f);
    [SerializeField] private Color cargedColor = Color.cyan;
    [SerializeField] private Image containerImage;

    public override void SetUpUI(BattleUnitBase unit )
    {
        AllyUnit ally = unit as AllyUnit;
        maxHealth = ally.CharacterData.MaxHealth;
        //atk.text = ally.CurrentAttack.ToString();
        //shd.text = ally.CurrentShield.ToString();
        //spd.text = ally.CurrentSpeed.ToString();

        base.SetUpUI(unit);
    }

    public override void SetHealth(int currentHealth)
    {
        float sliderValue = (float) currentHealth / maxHealth;
        healthSlider.value = sliderValue;
        healthTxt.text = $"{currentHealth} / {maxHealth}";
    }

    public void ReadyGnosis(bool enabled)
    {
        containerImage.enabled = enabled;
    }
}
