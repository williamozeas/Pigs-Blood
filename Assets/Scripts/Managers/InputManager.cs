using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Input Class used for non-mouse inputs
public class InputManager : Singleton<InputManager>
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Advance"))
        {
            GameManager.TriggerAdvance();
        }
    }
}
