using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Encounter Data", menuName = "Proto2/Encounter")]
public class EncounterData : ScriptableObject
{
    [SerializeField] private List<EnemyData> enemyList;
    public List<EnemyData> EnemyList => enemyList;
}
