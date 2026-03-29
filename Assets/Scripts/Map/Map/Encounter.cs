using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Encounter : MonoBehaviour
{
    [Header("EncounterData")]
    [SerializeField] private EncounterBase encounterData;

    public void OnClick(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
