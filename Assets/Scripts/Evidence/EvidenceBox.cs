using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EvidenceBox : MonoBehaviour
{
    private bool isColliding = false;
    private bool ignoring = false;
    
    private Transform[] borders;
    private Collider coll;

    private void Awake()
    {
        borders = GetComponentsInChildren<Transform>();
        coll = GetComponent<Collider>();
        SetActiveState(false);
    }

    private void OnEnable()
    {
        GameManager.ChangePlayerState += OnChangePlayerState;
    }
    
    private void OnDisable()
    {
        GameManager.ChangePlayerState -= OnChangePlayerState;
    }

    private void OnChangePlayerState(PlayerState newState)
    {
        switch (newState)
        {
            case (PlayerState.ChooseEvidence):
            {
                SetActiveState(true);
                break;
            }
            case (PlayerState.Inspecting):
            {
                break;
            }
            default:
            {
                SetActiveState(false);
                break;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isColliding || ignoring) return;
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

    IEnumerator IgnoreCollisions(float time)
    {
        ignoring = true;
        yield return new WaitForSeconds(time);
        ignoring = false;
    }

    void SetActiveState(bool newState)
    {
        foreach (Transform obj in borders)
        {
            if(obj != transform)
                obj.gameObject.SetActive(newState);
        }

        StartCoroutine(IgnoreCollisions(1f));
        coll.enabled = newState;
    }
}
