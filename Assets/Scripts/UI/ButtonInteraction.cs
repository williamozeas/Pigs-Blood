using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  

public class ButtonInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void clickToStartGame() {
        SceneManager.LoadScene("Interrogation");
    }
    
    public void clickToRestart() {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Interrogation");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
