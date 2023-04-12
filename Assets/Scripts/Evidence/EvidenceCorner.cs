using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EvidenceCorner : MonoBehaviour
{
    [FormerlySerializedAs("direction")] public PageDirection pageDirection;
    private IFlippable evidence;

    private void Awake()
    {
        evidence = GetComponentInParent<IFlippable>();
    }

    public void OnClick()
    {
        evidence.TryFlip(pageDirection);
    }
}
