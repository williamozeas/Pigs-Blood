using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A4 : EvidenceAbstract
{
    [TextArea(3, 10)] public List<string> pages;
    
    protected override void Awake ()
    {
        base.Awake();
        type = DocType.A4;
    }

    public override void Populate()
    {
        
    }
}
