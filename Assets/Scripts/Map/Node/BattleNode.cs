using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleNode : NodeBase
{
    [Header("Incounter")]
    [SerializeField] private List<BattleIncounter> incounters;

    public override void OnClick(string sceneName)
    {
        base.OnClick(sceneName);              //RandomEnemy()로 적 정보 넘겨줘야 함
        Debug.Log("Battle Button Access");
    }

    private BattleIncounter RandomIncounter()
    {
        int index = Random.Range(0, incounters.Count);
        return incounters[index];
    }
}
