using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    [Header("Encounter")]
    [SerializeField] private List<Encounter> encounters = new List<Encounter>();

    private EncounterBase encounterData = null;

    public void SetEncounterData(EncounterBase inEncounter) {  encounterData = inEncounter; }
    public EncounterBase GetEncounterData() {  return encounterData; }

    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    private void LoadMap(MapData mapData)
    {
        /* 맵 씬에 미리 있어야 할 것 : Encounter Container, Path Container 등
         * 맵 씬의 AudioSource 컴포넌트, 배경 이미지 컴포넌트 등을 미리 MapManager에 인스펙터로 등록하거나 Awake단에서 확보한다.
         * mapData의 비주얼 세팅을 하고, 각 컨테이너 아래에 Passage와 Encounter을 스폰한다.
         * 로드가 다 되면 상단 텍스트 UI에 지역과 섹터를 2초정도 출력한다.
         */
    }

    private void LoadBattle(EnemyEncounter encounter)
    {
        //GameManager.Instance.CurrentEncounter = encounter     //DontDestroyOnLoad인 GameManager, CurrentRun 등의 데이터에 복사해두기.
        SceneManager.LoadScene("BattleScene");
        //이후 BattleManager Awake에서 encounter에 필요한 데이터를 GameManager.Instance,CurrentEncounter에서 다시 복사하여 배틀세팅을 한다.
        //배틀이 로드 되는지, 로드 된 배틀이 인카운터 정보를 충실히 반영하고 있는지 테스트해야 한다.
    }
}
