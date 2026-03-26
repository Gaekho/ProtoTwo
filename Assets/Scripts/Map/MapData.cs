using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map Data", menuName = "Proto2/MapData", order = 0)]
public class MapData : ScriptableObject
{
    [SerializeField] private int id;

    [Header("Text settings")]
    [SerializeField] private string region; //enum으로 변경 가능성 높음.
    [SerializeField] private string sector;

    [Header("Outputs")]
    [SerializeField] private Sprite mapSprite;
    [SerializeField] private AudioClip bGM;
    //[SerializeField] List<> private AnimatingObjecst;     //아직 어떻게 하는지 모름.중요하지 않다.

    [Header("Passage to other Room")]
    [SerializeField] private List<MapPassage> mapPassages = new();

    [Header("Enemy Encounters of this Room")]
    [SerializeField] private List<EnemyEncounter> enemyEncounters = new();

    #region Cache

    #endregion
}

[Serializable]
public class MapPassage
{
    [SerializeField] private Vector3 spawnTransform;
    [SerializeField] private Sprite sprite;
    [SerializeField] private MapData toWhere;
}

[Serializable]
public class EnemyEncounter
{
    [SerializeField] private Vector3 spawnTransform;
    [SerializeField] private Sprite representSprite;
    [SerializeField] private List<EnemyData> enemies;
}