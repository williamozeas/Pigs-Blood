using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class TextLineView : LineView
{
    [SerializeField] private float fadeTime = 1.2f;
    private string currentLine;
    private int charIndex;

    [SerializeField] private Image ShepherdDialogueBox;
    private Coroutine FadeCR;
    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        base.RunLine(dialogueLine, onDialogueLineFinished);
        //SFX
        currentLine = dialogueLine.RawText;
        charIndex = 0;
        
        //Text box
        if (dialogueLine.CharacterName == "Shepherd" || dialogueLine.CharacterName == "Boy")
        {
            if (FadeCR != null)
            {
                StopCoroutine(FadeCR);
            }
            FadeCR = StartCoroutine(FadeDialogueBox(1, fadeTime));
        }
        else
        {
            if (FadeCR != null)
            {
                StopCoroutine(FadeCR);
            }
            FadeCR = StartCoroutine(FadeDialogueBox(0, fadeTime));
        }
    }

    public void PlayTextSFX()
    {
        if (currentLine[charIndex] != ' ')
        {
            AudioManager.Instance.CharacterTypedSound();
        }
        charIndex++;
    }

    public override void UserRequestedViewAdvancement()
    {
        base.UserRequestedViewAdvancement();
        AudioManager.Instance.ContinueTextSound();
    }

    private IEnumerator FadeDialogueBox(float goalAlpha, float time)
    {
        float timeElapsed = 0;
        float startAlpha = ShepherdDialogueBox.color.a;
        float diff = goalAlpha - startAlpha;
        if (Mathf.Abs(diff) < 0.1f)
        {
            yield break;
        }
        while (timeElapsed < time)
        {
            float percent = EasingFunction.EaseOutQuad(0, 1, timeElapsed / time);
            ShepherdDialogueBox.color = new Color(1, 1, 1, startAlpha + percent * diff);
            Debug.Log(startAlpha + percent * diff);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        ShepherdDialogueBox.color = new Color(1, 1, 1, goalAlpha);
    }
}
