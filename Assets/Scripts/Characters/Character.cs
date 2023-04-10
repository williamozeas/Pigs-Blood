using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected Renderer[] renderers;
    protected Animator animator;
    
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TriggerPose(string pose)
    {
        if (animator)
        {
            animator.SetTrigger(pose);
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
