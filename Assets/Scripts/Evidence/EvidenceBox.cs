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
        EvidenceAbstract doc = other.gameObject.GetComponent<EvidenceAbstract>();
        if (doc)
        {
            doc.OnPresent();
        }

        StartCoroutine(Reset());
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(1f);
        isColliding = false;
    }
}
