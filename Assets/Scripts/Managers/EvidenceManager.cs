using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EvidenceManager : Singleton<EvidenceManager>
{
    private List<EvidenceAbstract> evidence;
    [SerializeField] private Transform resetPoint;

    public override void Awake()
    {
        base.Awake();
        evidence = GetComponentsInChildren<EvidenceAbstract>().ToList();
    }

    public EvidenceAbstract GetEvidenceByType(DocType type)
    {
        return evidence.Find((evidence => evidence.Type == type));
    }
    
    public EvidenceAbstract GetEvidenceByID(string id)
    {
        return evidence.Find((evidence => evidence.id == id));
    }

    public void ResetEvidence(EvidenceAbstract evidenceToReset)
    {
        evidenceToReset.transform.position = resetPoint.position;
    }
}
