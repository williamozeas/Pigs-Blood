using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableA4 : GrabbableAbstract
{
    protected override void Awake ()
    {
        base.Awake();
        type = DocType.A4;
    }
}
