using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlashImage : MonoBehaviour
{
    // private Coroutine flashCoroutine;
    private Image img;
    void Awake()
    {
        img = GetComponent<Image>();
        CameraManager.Instance.flash = this;
    }

    public void Flash(float attackTime, float sustainTime, float decayTime, Color fadeColor, float delay)
    {
        StartCoroutine(FlashCR(attackTime, sustainTime, decayTime, fadeColor, delay));
    }

    private IEnumerator FlashCR(float attackTime, float sustainTime, float decayTime, Color fadeColor, float delay)
    {
        yield return new WaitForSeconds(delay);
        float timeElapsed = 0;
        while (timeElapsed < attackTime)
        {
            float alpha = EasingFunction.EaseInQuad(0, 1, timeElapsed / attackTime);
            Color newColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);
            img.color = newColor;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        img.color = fadeColor;

        if (decayTime < -0.5f)
        {
            yield break;
        }
        yield return new WaitForSeconds(sustainTime);
        
        timeElapsed = 0;
        while (timeElapsed < decayTime)
        {
            float alpha = EasingFunction.EaseOutQuad(1, 0, timeElapsed / decayTime);
            Color newColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);
            img.color = newColor;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        img.color = new Color(1, 1, 1, 0);
    }
}
