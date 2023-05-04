using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EvidenceManager : Singleton<EvidenceManager>
{
    private List<EvidenceAbstract> currentEvidence;
    [SerializeField] private EvidenceDatabase evidenceDatabase;
    private Dictionary<string, GameObject> evidenceDict = new Dictionary<string, GameObject>();
    [SerializeField] private Transform resetPoint;
    EvidenceAbstract grabbedObject;
    private EvidenceAbstract inspectedObject;
    private Camera mainCam;

    public override void Awake()
    {
        base.Awake();
        currentEvidence = GetComponentsInChildren<EvidenceAbstract>().ToList();
        // string[] evidenceGuids = AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/Prefabs/PremadeEvidence" });
        foreach (EvidenceAbstract evidence in evidenceDatabase.EvidenceList)
        {
            // string path = AssetDatabase.GUIDToAssetPath(guid);
            // GameObject evidence = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            EvidenceAbstract evClass = evidence.GetComponent<EvidenceAbstract>();
            if (!evClass)
            {
                Debug.LogError("Invalid evidence loaded!");
            }
            evidenceDict.Add(evClass.id, evidence.gameObject);
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
            
            //raycast for corners
            if (Physics.Raycast(ray, out raycastHit, 10f, LayerMask.GetMask("Corner"), QueryTriggerInteraction.Ignore))
            {
                if (raycastHit.transform != null && inspectedObject)
                {
                    EvidenceCorner corner = raycastHit.collider.GetComponent<EvidenceCorner>();
                    Debug.Log(corner);
                    corner.OnClick();
                }
            } 
            else if (Physics.Raycast(ray, out raycastHit, 10f, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
            {
                if (raycastHit.transform != null && inspectedObject && 
                    raycastHit.transform.gameObject != inspectedObject.gameObject && 
                    inspectedObject.canBeClickedOff == true)
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

    public void LoadEvidence(string id)
    {
        if (GetEvidenceByID(id) != null)
        {
            Debug.LogWarning("Duplicate evidence added: " + id);
            return;
        }
        GameObject prefab = GetEvidencePrefabByID(id);
        float x = Random.Range(-2f, 2f);
        float z = Random.Range(-3.2f, -5f);
        Vector3 position = new Vector3(x, 3.6f, z);
        Quaternion rotation = Quaternion.Euler(0f, Random.Range(-30f, 30f), 0f);
        GameObject newEvidence = Instantiate(prefab, position, rotation, transform);
        currentEvidence.Add(newEvidence.GetComponent<EvidenceAbstract>());
    }

    public void RemoveEvidence(string id)
    {
        EvidenceAbstract evidenceToRemove = GetEvidenceByID(id);
        if (evidenceToRemove)
        {
            currentEvidence.Remove(evidenceToRemove);
            Destroy(evidenceToRemove.gameObject);
        }
    }

    public void RemoveAllEvidence()
    {
        EvidenceAbstract nb = GetCurrentEvidenceByType(DocType.Notebook);
        foreach (var evidenceToRemove in currentEvidence)
        {
            if(evidenceToRemove != nb)
                Destroy(evidenceToRemove.gameObject);
        }
        currentEvidence.Clear();
        currentEvidence.Add(nb);
    }

    public Vector3 GetResetPoint()
    {
        return resetPoint.position;
    }
}
