using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDocuments : MonoBehaviour
{
    EvidenceAbstract grabbedObject;
    private Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if ((GameManager.Instance.PlayerState == PlayerState.Review || GameManager.Instance.PlayerState == PlayerState.ChooseEvidence) 
            && Input.GetMouseButtonDown(0))
        {
            RaycastHit raycastHit;
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out raycastHit, 10f))
            {
                if (raycastHit.transform != null)
                {
                    if (raycastHit.collider.gameObject.TryGetComponent(out EvidenceAbstract grabbableObject))
                    {
                        Vector3 screenPoint = mainCam.WorldToScreenPoint(gameObject.transform.position);
                
                        Vector3 offset = gameObject.transform.position - mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
                
                        grabbableObject.Grabbed(gameObject, offset);
                        grabbedObject = grabbableObject;
                    }
                }
            }

        }

        if (Input.GetMouseButtonUp(0) && grabbedObject != null)
        {
            grabbedObject.Dropped();
            grabbedObject = null;
        }
    }
}