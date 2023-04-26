using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PosePair
{
    public string poseName;
    public GameObject poseObject;
}

public abstract class Character : MonoBehaviour
{
    protected Renderer[] renderers;
    protected Animator animator;

    public List<PosePair> poseObjects = new List<PosePair>();
    Dictionary<string, GameObject> poseDict = new Dictionary<string, GameObject>();
    GameObject currentPose;

    
    // Start is called before the first frame update
    protected void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }
        
        foreach (PosePair pair in poseObjects)
        {
            poseDict.Add(pair.poseName, pair.poseObject);
        }

        Debug.Log(gameObject.name);
        currentPose = poseObjects[0].poseObject;
        currentPose.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TriggerPose(string pose)
    {
        if (poseDict.ContainsKey(pose))
        {
            currentPose.SetActive(false);
            currentPose = poseDict[pose];
            currentPose.SetActive(true);
            Debug.Log("Set pose: " + name + " " + pose);
        }
        else
        {
            Debug.Log("Set nonexistent pose: " + name + " " + pose);
        }
    }

    public virtual void FadeIn()
    {
        //TODO: Make actual fade
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = true;
        }
    }

    public virtual void FadeOut()
    {
        //TODO: Make actual fade
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }
    }
}
