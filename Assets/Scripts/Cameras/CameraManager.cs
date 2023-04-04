using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    public CinemachineVirtualCamera talkCam;
    public CinemachineVirtualCamera docsCam;
    
    // Start is called before the first frame update
    void Start()
    {
        OnChangePlayerState(GameManager.Instance.PlayerState);
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
        if (newState == PlayerState.Review)
        {
            if(docsCam)
                docsCam.enabled = true;
            if(talkCam)
                talkCam.enabled = false;
        }
        else
        {
            if(docsCam)
                docsCam.enabled = false;
            if(talkCam)
                talkCam.enabled = true;
        }
    }
}
