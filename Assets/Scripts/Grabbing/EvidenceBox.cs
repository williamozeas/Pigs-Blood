using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceBox : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        GrabbableAbstract doc = other.gameObject.GetComponent<GrabbableAbstract>();
        if (doc)
        {
            GameManager.Instance.YarnCommandManager.RespondToEvidence(doc.id);
        }
    }
}
