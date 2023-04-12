using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PageAbstract : MonoBehaviour //could be interface?
{
    //populate text fields
    public abstract void Populate();
    
    //deactivate text etc on page
    public abstract void Deactivate();
}
