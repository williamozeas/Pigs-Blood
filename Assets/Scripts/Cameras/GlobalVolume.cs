using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class GlobalVolume : MonoBehaviour
{
    private void Awake()
    {
        CameraManager.Instance.volume = GetComponent<Volume>();
    }
}
