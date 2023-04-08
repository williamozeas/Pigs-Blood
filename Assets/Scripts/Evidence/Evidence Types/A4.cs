using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A4 : EvidenceAbstract
{
    protected override void Awake ()
    {
        base.Awake();
        type = DocType.A4;
    }
}
