using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Encounter : MonoBehaviour
{
    [Header("EncounterData")]
    [SerializeField] private EncounterBase encounterData;
    [SerializeField] private string sceneName = null;
    [SerializeField] private KeyCode interactKey = KeyCode.F;

    private bool isActivated = false;

    private void Update()
    {
        if(isActivated && Input.GetKeyDown(interactKey))
        {
            Interactive();
        }
    }

    private void Interactive()
    {
        if (sceneName != null)
        {
            GameManager.Instance.SetEncounterData(encounterData);

            SceneManager.LoadScene(sceneName);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something IN");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player IN");
            isActivated = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Something Out");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Out");
            isActivated = false;
        }
    }
}
