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
            
            if (Physics.Raycast(ray, out raycastHit, 10f, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
            {
                if (raycastHit.transform != null)
                {
                    Debug.Log(raycastHit.transform.name);
                    if (raycastHit.collider.gameObject.TryGetComponent(out EvidenceAbstract grabbableObject))
                    {
                        Vector3 screenPoint = mainCam.WorldToScreenPoint(gameObject.transform.position);
                
                        Vector3 offset = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z)) - transform.position;

                        grabbableObject.Grabbed(gameObject, Vector3.zero);
                        grabbedObject = grabbableObject;
                    }
                }
            }

        }

        if ((GameManager.Instance.PlayerState == PlayerState.Review || GameManager.Instance.PlayerState == PlayerState.ChooseEvidence)
            &&Input.GetMouseButtonUp(0) && grabbedObject != null )
        {
            grabbedObject.Dropped();
            grabbedObject = null;
        }

        if (GameManager.Instance.PlayerState == PlayerState.Inspecting && Input.GetKeyDown("d"))
        {
            Debug.Log("aaa");
            grabbedObject.UnInspect();
            Debug.Log("bbb");
            grabbedObject.Dropped();
            Debug.Log("ccc");
            grabbedObject = null;
            Debug.Log("ddd");
        }
    }
}