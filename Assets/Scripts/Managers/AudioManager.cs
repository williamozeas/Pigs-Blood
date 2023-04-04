using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioManager : Singleton<AudioManager>
{
    private EventInstance Music;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetMusic(string name) //TODO: Don't change music if it's the same 
    {
        StopMusic();

        string eventString = "event:/Music/" + name;
        Music = RuntimeManager.CreateInstance(eventString);
        Music.start();
    }

    public void StopMusic()
    {
        if (Music.isValid())
        {
            Music.release();
            Music.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}
