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
    [SerializeField] private List<BattleUnitBase> waitingUnits = new();


    public void ReBuildQueue(int queueCount, List<BattleUnitBase> orderedUnits)
    {
        ClearQueueUI();
        SetQueueCount(queueCount);

        if (orderedUnits == null || orderedUnits.Count == 0) return;

        //첫번째 고정
        SetQelement(turnQElement, orderedUnits[0]);
        turnQElement.transform.localScale = turnScale;

        //두번째부터 생성
        for(int i=1; i<orderedUnits.Count; i++)
        {
            CreateWaitingElement(orderedUnits[i]);
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

    public void TransferQueueUI()
    {
        if(spawnedQElements.Count == 0) return;

        GameObject next = spawnedQElements[0];
        BattleUnitBase nextUnit = waitingUnits[0];

        SetQelement(turnQElement, nextUnit);
        turnQElement.transform.localScale = turnScale;

        spawnedQElements.RemoveAt(0);
        waitingUnits.RemoveAt(0);
        Destroy(next);
    }

    public void RemoveUnitFromQueueUI(BattleUnitBase deadUnit)
    {
        if (deadUnit == null) return;

        int idx = waitingUnits.IndexOf(deadUnit);
        if (idx < 0) return;

        if (idx < spawnedQElements.Count && spawnedQElements[idx] != null)
        {
            Destroy(spawnedQElements[idx]);
        }

        waitingUnits.RemoveAt(idx);
        spawnedQElements.RemoveAt(idx);
    }

    private void SetQueueCount(int count)
    {
        queueCount.text = $"[ {count} ]";
    }

    private void CreateWaitingElement(BattleUnitBase unit)
    {
        GameObject newElement = Instantiate(qElement, queueContainer);
        spawnedQElements.Add(newElement);
        waitingUnits.Add(unit);

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
