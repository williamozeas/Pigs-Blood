using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class TextLineView : LineView
{
    [SerializeField] private float fadeTime = 1.2f;
    private string currentLine;
    private int charIndex;

    private string prevCharacterName = "????";

    [SerializeField] private Image ShepherdDialogueBox;
    [SerializeField] private TextMeshProUGUI CharacterNameText;
    [SerializeField] private TextMeshProUGUI tmp;
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
            FadeCR = StartCoroutine(FadeDialogueBox(1, 0.45f, fadeTime));
            CharacterNameText.text = prevCharacterName;
        }
        else
        {
            prevCharacterName = dialogueLine.CharacterName;
            if (FadeCR != null)
            {
                StopCoroutine(FadeCR);
            }
            FadeCR = StartCoroutine(FadeDialogueBox(0, 1f, fadeTime));
        }
    }

    public void PlayTextSFX()
    {
        if (charIndex < currentLine.Length && currentLine[charIndex] != ' ')
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

    private IEnumerator FadeDialogueBox(float goalAlphaBox, float goalAlphaName, float time)
    {
        float timeElapsed = 0;
        float startAlphaBox = ShepherdDialogueBox.color.a;
        float diffBox = goalAlphaBox - startAlphaBox;
        float startAlphaName = CharacterNameText.color.a;
        float diffName = goalAlphaName - startAlphaName;
        if (Mathf.Abs(diffBox) < 0.1f)
        {
            yield break;
        }
        while (timeElapsed < time)
        {
            float percent = EasingFunction.EaseOutQuad(0, 1, timeElapsed / time);
            ShepherdDialogueBox.color = new Color(1, 1, 1, startAlphaBox + percent * diffBox);
            tmp.color = new Color(1, 1, 1, startAlphaName + percent * diffName);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        ShepherdDialogueBox.color = new Color(1, 1, 1, goalAlphaBox);
        tmp.color = new Color(1, 1, 1, goalAlphaName);
    }
}
