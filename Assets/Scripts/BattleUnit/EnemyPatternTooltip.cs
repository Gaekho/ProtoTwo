using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyPatternTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image tooltipBox;
    [SerializeField] private TMP_Text patternName;
    [SerializeField] private TMP_Text patternDescription;

    public void SetTooltip(EnemyPatternData currentPattern)
    {
        patternName.text = currentPattern.PatternName;
        patternDescription.text = currentPattern.PatternDescription;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipBox.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipBox.gameObject.SetActive(false);
    }
}
