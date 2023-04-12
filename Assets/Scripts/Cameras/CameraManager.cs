using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CameraManager : Singleton<CameraManager>
{
    public float camTransitionTime = 1.2f;
    public float reviewPaniniProj = 0.15f;
    public float inspectTransitionTime = 0.6f;
    
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
            {
                if(docsCam)
                    docsCam.enabled = true;
                if(talkCam)
                    talkCam.enabled = false;

                float transitionTimeFinal = camTransitionTime;
                if (GameManager.Instance.PlayerState == PlayerState.Inspecting)
                {
                    transitionTimeFinal = inspectTransitionTime;
                }
                if (paniniProjection)
                {
                    StartCoroutine(ChangePaniniProjection(reviewPaniniProj, transitionTimeFinal));
                }
                break;
            }
            case PlayerState.Inspecting:
            {
                if (paniniProjection)
                {
                    StartCoroutine(ChangePaniniProjection(0.03f, inspectTransitionTime));
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
                    StartCoroutine(ChangePaniniProjection(startPaniniProjectionDistance, camTransitionTime));
                }
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
            SetPaniniProjectionTransition(startDistance, targetDistance, timeElapsed / time);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        paniniProjection.distance.value = targetDistance;
    }

    private void SetPaniniProjectionTransition(float start, float end, float percent)
    {
        float newDistance = EasingFunction.EaseOutQuad(start, end, percent);
        paniniProjection.distance.value = newDistance;
    }

    public void SetPaniniProjectionTransitionInspect(float percent)
    {
        SetPaniniProjectionTransition(reviewPaniniProj, 0.05f, percent);
    }
}
