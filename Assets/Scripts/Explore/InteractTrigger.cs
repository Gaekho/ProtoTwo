using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTrigger : MonoBehaviour
{
    [Header("Interaction KEY")]
    [SerializeField] private string interactionKey;
    public void TriggerDialogue()
    {
        NodeDirector director = gameObject.GetComponentInParent<NodeDirector>();
        director.InteractionProcess(interactionKey);
    }
}
