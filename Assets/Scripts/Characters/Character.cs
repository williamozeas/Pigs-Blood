using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected MeshRenderer[] renderers;
    
    // Start is called before the first frame update
    protected void Awake()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            Debug.Log(gameObject.name);
            renderer.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void TriggerPose(string pose);

    public virtual void FadeIn()
    {
        //TODO: Make actual fade
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.enabled = true;
        }
    }

    public virtual void FadeOut()
    {
        //TODO: Make actual fade
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.enabled = false;
        }
    }
}
