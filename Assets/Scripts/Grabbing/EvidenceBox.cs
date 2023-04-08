using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceBox : MonoBehaviour
{
    private bool isColliding = false;
    void OnTriggerEnter(Collider other)
    {
        if (isColliding) return;
        isColliding = true;
        GrabbableAbstract doc = other.gameObject.GetComponent<GrabbableAbstract>();
        if (doc)
        {
            GameManager.Instance.YarnCommandManager.RespondToEvidence(doc.id);
        }

        StartCoroutine(Reset());
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(1f);
        isColliding = false;
    }
}
