using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenContinueHitbox : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        GameManager.ChangePlayerState += OnChangePlayerState;
    }

    private void OnDisable()
    {
        GameManager.ChangePlayerState -= OnChangePlayerState;
    }

    private void OnChangePlayerState(PlayerState state)
    {
        switch (state)
        {
            case (PlayerState.Inspecting):
            case (PlayerState.Review):
            case (PlayerState.ChooseEvidence):
            {
                image.enabled = false;
                break;
            }
            case (PlayerState.Talk):
            {
                image.enabled = true;
                break;
            }
        }
    }
}
