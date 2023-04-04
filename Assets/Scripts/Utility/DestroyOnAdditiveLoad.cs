using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyOnAdditiveLoad : MonoBehaviour
{
    public string HomeScene = "Documents";
    
    // Start is called before the first frame update
    void Awake()
    {
        Scene currentScene = SceneManager.GetActiveScene ();
        if (currentScene.name != HomeScene)
        {
            DestroyImmediate(this.gameObject);
        }
    }
}
