using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//리스트로 적 정보를 담은 스크립터블 오브젝트
//스테이지당 하나씩 만들면 됨

public class BattleIncounter : ScriptableObject
{
    [Header("Incounter")]
    [SerializeField] private List<EnemyData> enemyData;

    public List<EnemyData> GetEnemies() { return enemyData; }
}
