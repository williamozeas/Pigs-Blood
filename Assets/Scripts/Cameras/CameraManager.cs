using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    public CinemachineVirtualCamera talkCam;
    public CinemachineVirtualCamera docsCam;
    public List<CinemachineShake> shakers = new List<CinemachineShake>();
    
    // Start is called before the first frame update
    void Start()
    {
        OnChangePlayerState(GameManager.Instance.PlayerState);
        shakers.Add(talkCam.GetComponent<CinemachineShake>());
        shakers.Add(docsCam.GetComponent<CinemachineShake>());
    }

    private void OnEnable()
    {
        GameManager.ChangePlayerState += OnChangePlayerState;
    }
    
    private void OnDisable()
    {
        GameManager.ChangePlayerState -= OnChangePlayerState;
    }

    public void Shake(float intensity, float time)
    {
        shakers.ForEach(shaker => shaker.ShakeCamera(intensity, time));
    }

    private void OnChangePlayerState(PlayerState newState)
    {
        switch (newState)
        {
            case PlayerState.Review:
            case PlayerState.ChooseEvidence:
            case PlayerState.Inspecting:
            {
                if(docsCam)
                    docsCam.enabled = true;
                if(talkCam)
                    talkCam.enabled = false;
                break;
            }
            default:
            case PlayerState.Talk:
            {
                if(docsCam)
                    docsCam.enabled = false;
                if(talkCam)
                    talkCam.enabled = true;
                break;
            }
        }
    }
}
