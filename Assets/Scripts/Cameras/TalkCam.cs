using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TalkCam : MonoBehaviour
{
    void Awake()
    {
        CameraManager.Instance.talkCam = GetComponent<CinemachineVirtualCamera>();
    }
}
