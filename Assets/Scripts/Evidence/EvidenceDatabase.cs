using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EvidenceDatabase", order = 1)]
public class EvidenceDatabase : ScriptableObject
{
    public List<EvidenceAbstract> EvidenceList;
}
