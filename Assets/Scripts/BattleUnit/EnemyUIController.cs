using Proto2.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIController : MonoBehaviour
{
    [Header("EnemyUI")]
    [SerializeField] private EnemyUnit owner;
    [SerializeField] private Canvas myCanvas;
    [SerializeField] private Text armorText;
    [SerializeField] private Image intentIcon;

    [Header("Health UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Text healthText;

    [Header("Buff UI")]
    [SerializeField] private Transform buffContainer;
    [SerializeField] private GameObject buffUI;

    private readonly Dictionary<BuffTypes, BuffUI> buffUIs = new();

    public void SetUpUI(EnemyUnit unit)
    {
        owner = unit;
        myCanvas = transform.GetComponent<Canvas>();
        armorText.text = "0";
        intentIcon.sprite = null;

        healthSlider.value = 1f;
        healthText.text = unit.CurrentHealth.ToString() + "/" + unit.EnemyData.MaxHealth;
    }

    public void SetHealth(float currentHealth)
    {
        healthSlider.value = currentHealth / owner.EnemyData.MaxHealth;
        healthText.text = owner.CurrentHealth.ToString() +"/"+ owner.EnemyData.MaxHealth;
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
    public void CreateOrRefreshBuffUI(BuffInstance buff)
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
}
