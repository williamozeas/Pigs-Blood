using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EvidenceManager : Singleton<EvidenceManager>
{
    private List<EvidenceAbstract> evidence;
    [SerializeField] private Transform resetPoint;
    EvidenceAbstract grabbedObject;
    private EvidenceAbstract inspectedObject;
    private Camera mainCam;

    public override void Awake()
    {
        base.Awake();
        evidence = GetComponentsInChildren<EvidenceAbstract>().ToList();
    }
    
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
                    }
                }
            }

        }

        if (GameManager.Instance.PlayerState == PlayerState.Inspecting && Input.GetMouseButtonDown(0))
        {
            RaycastHit raycastHit;
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out raycastHit, 10f, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
            {
                if (raycastHit.transform != null && inspectedObject && raycastHit.transform.gameObject != inspectedObject.gameObject)
                {
                    inspectedObject.UnInspect();
                    inspectedObject = null;
                }
            }
        }
    }

    public void SetInspectedObject(EvidenceAbstract newlyGrabbed)
    {
        inspectedObject = newlyGrabbed;
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
