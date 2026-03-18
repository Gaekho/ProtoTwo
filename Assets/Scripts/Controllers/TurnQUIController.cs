using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Proto2.Enums;
using UnityEngine.UI;

public class TurnQUIController : MonoBehaviour
{
    [Header("Fixed UI")]
    [SerializeField] private TMP_Text queueCount;
    [SerializeField] private GameObject turnQElement;

    [Header("Q Element Prefab")]
    [SerializeField] private GameObject qElement;

    [Header("Queue Container")]
    [SerializeField] private Transform queueContainer;

    [Header("Visual Setting")]
    [SerializeField] private Color enemyColor = Color.gray;
    [SerializeField] private Vector3 normalScale = Vector3.one;
    [SerializeField] private Vector3 turnScale = new (1.2f, 1.2f);

    [Header("Runtime")]
    [SerializeField] private List<GameObject> spawnedQElements = new();


    public void ReBuildQueue(int queueCount, List<BattleUnitBase> aliveUnits)
    {
        ClearQueueUI();
        SetQueueCount(queueCount);

        if (aliveUnits == null || aliveUnits.Count == 0) return;

        //첫번째 고정
        SetQelement(turnQElement, aliveUnits[0]);
        turnQElement.transform.localScale = turnScale;

        //두번째부터 생성
        for(int i=1; i<aliveUnits.Count; i++)
        {
            CreateWaitingElement(aliveUnits[i]);
        }
    }

    public void ClearQueueUI()
    {
        for(int i=0; i<spawnedQElements.Count; i++)
        {
            if (spawnedQElements[i] != null) Destroy(spawnedQElements[i]);

            spawnedQElements.Clear();
        }
    }
    private void SetQueueCount(int count)
    {
        queueCount.text = $"[ {count} ]";
    }

    public void OnUnitTurnStart()
    {
        if(spawnedQElements.Count == 0) return;

        GameObject next = spawnedQElements[0];
        if(next == null)
        {
            spawnedQElements.RemoveAt(0);
            return;
        }

        //턴 슬롯으로 비주얼 이동
        turnQElement.GetComponent<Image>().sprite = next.GetComponent<Image>().sprite;
        turnQElement.GetComponent<Outline>().effectColor = next.GetComponent<Outline>().effectColor;

        //원본삭제
        spawnedQElements.RemoveAt(0);
        Destroy(next);
    }

    private void CreateWaitingElement(BattleUnitBase unit)
    {
        GameObject newElement = Instantiate(qElement, queueContainer);
        spawnedQElements.Add(newElement);
        SetQelement(newElement, unit);
        newElement.transform.localScale = normalScale;
    }

    private void SetQelement(GameObject element, BattleUnitBase unit)
    {
        Image image = element.GetComponent<Image>();
        Outline outline = element.GetComponent<Outline>();

        if(unit.Team == UnitTeam.Ally)
        {
            AllyUnit ally = unit as AllyUnit;
            image.sprite = ally.CharacterData.ThumbNail;
            outline.effectColor = ally.CharacterData.UIColor;
        }
        else if(unit.Team == UnitTeam.Enemy)
        {
            EnemyUnit enemy = unit as EnemyUnit;
            image.sprite = enemy.EnemyData.ThumbNail;
            outline.effectColor = enemyColor;
        }
    }

}
