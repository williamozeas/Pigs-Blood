using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class TextLineView : LineView
{
    private string currentLine;
    private int charIndex;
    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        base.RunLine(dialogueLine, onDialogueLineFinished);
        currentLine = dialogueLine.RawText;
        charIndex = 0;
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
}
