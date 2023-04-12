using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class CameraManager : Singleton<CameraManager>
{
    public float transitionTime = 1.3f;
    
    [HideInInspector] public ScreenFlashImage flash;
    [HideInInspector] public Camera mainCam;
    [HideInInspector] public CinemachineVirtualCamera talkCam;
    [HideInInspector] public CinemachineVirtualCamera docsCam;
    [HideInInspector] public Volume volume;
    [HideInInspector] public List<CinemachineShake> shakers = new List<CinemachineShake>();

    private PaniniProjection paniniProjection;
    private float startPaniniProjectionDistance;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        OnChangePlayerState(GameManager.Instance.PlayerState);
        shakers.Add(talkCam.GetComponent<CinemachineShake>());
        shakers.Add(docsCam.GetComponent<CinemachineShake>());
        if (!volume.profile.TryGet<PaniniProjection>(out paniniProjection))
        {
            Debug.Log("Get Panini Projection failed");
        }

        startPaniniProjectionDistance = paniniProjection.distance.value;
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

    public void Flash(float attack, float sustain, float release, Color flashColor, float delay = 0)
    {
        flash.Flash(attack, sustain, release, flashColor, delay);
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
                
                if (paniniProjection && paniniProjection.distance.value > 0)
                {
                    StartCoroutine(ChangePaniniProjection(0, transitionTime));
                }
                break;
            }
            default:
            case PlayerState.Talk:
            {
                if(docsCam)
                    docsCam.enabled = false;
                if(talkCam)
                    talkCam.enabled = true;
                
                if (paniniProjection && paniniProjection.distance.value < startPaniniProjectionDistance)
                {
                    StartCoroutine(ChangePaniniProjection(startPaniniProjectionDistance, transitionTime));
                }
                break;
                break;
            }
        }
    }

    private IEnumerator ChangePaniniProjection(float targetDistance, float time)
    {
        float timeElapsed = 0;
        float startDistance = paniniProjection.distance.value;
        while (timeElapsed < time)
        {
            float newDistance = EasingFunction.EaseOutQuad(startDistance, targetDistance, timeElapsed / time);
            paniniProjection.distance.value = newDistance;
            Debug.Log(newDistance);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        paniniProjection.distance.value = targetDistance;
    }
}
