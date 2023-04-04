using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DocsCam : MonoBehaviour
{
    void Awake()
    {
        CameraManager.Instance.docsCam = GetComponent<CinemachineVirtualCamera>();
    }
}
