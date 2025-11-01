using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Ritual Card Data", menuName = "Proto2/Card/RitualCard", order = 0)]

public class RitualCardData : CardData
{
    [Header("Ritual Setting")]
    public int GnosisCost;
}
