using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class A4Page : PageAbstract
{
    public string pageText;
    public TextMeshProUGUI tmp;
    public TMP_FontAsset font;
    public float spacing;

    public float fontSize = 6;
    // Start is called before the first frame update
    void Awake()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }

    public override void Populate()
    {
        tmp.font = font;
        tmp.lineSpacing = spacing;
        tmp.fontSize = fontSize;
        tmp.text = pageText;
    }

    public override void Deactivate()
    {
        tmp.enabled = false;
    }
}
