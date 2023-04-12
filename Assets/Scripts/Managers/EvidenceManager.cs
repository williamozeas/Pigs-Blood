using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EvidenceManager : Singleton<EvidenceManager>
{
    private List<EvidenceAbstract> currentEvidence;
    private Dictionary<string, GameObject> evidenceDict = new Dictionary<string, GameObject>();
    [SerializeField] private Transform resetPoint;
    EvidenceAbstract grabbedObject;
    private EvidenceAbstract inspectedObject;
    private Camera mainCam;

    public override void Awake()
    {
        base.Awake();
        currentEvidence = GetComponentsInChildren<EvidenceAbstract>().ToList();
        string[] evidenceGuids = AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/Prefabs/PremadeEvidence" });
        foreach (string guid in evidenceGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject evidence = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            EvidenceAbstract evClass = evidence.GetComponent<EvidenceAbstract>();
            if (!evClass)
            {
                Debug.LogError("Invalid evidence loaded!");
            }
            evidenceDict.Add(evClass.name, evidence);
        }
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
        
        //exiting inspect by clicking off
        if (GameManager.Instance.PlayerState == PlayerState.Inspecting && Input.GetMouseButtonDown(0))
        {
            RaycastHit raycastHit;
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out raycastHit, 10f, LayerMask.GetMask("RCorner"), QueryTriggerInteraction.Ignore))
            {
                if (raycastHit.transform != null && inspectedObject)
                {
                    //flip right
                    inspectedObject.TryFlip(true);
                }
            } else if (Physics.Raycast(ray, out raycastHit, 10f, LayerMask.GetMask("LCorner"), QueryTriggerInteraction.Ignore))
            {
                if (raycastHit.transform != null && inspectedObject)
                {
                    //flip left
                    inspectedObject.TryFlip(false);
                }
            } else if (Physics.Raycast(ray, out raycastHit, 10f, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
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

    public EvidenceAbstract GetCurrentEvidenceByType(DocType type)
    {
        return currentEvidence.Find((evidence => evidence.Type == type));
    }
    
    public EvidenceAbstract GetEvidenceByID(string id)
    {
        return currentEvidence.Find((evidence => evidence.id == id));
    }

    public void ResetEvidence(EvidenceAbstract evidenceToReset)
    {
        evidenceToReset.transform.position = resetPoint.position;
    }

    public GameObject GetEvidencePrefabByID(string id)
    {
        GameObject prefab;
        if (!evidenceDict.TryGetValue(id, out prefab))
        {
            Debug.LogError("Invalid evidence ID gotten: " + id);
        }
        return prefab;
    }
}
