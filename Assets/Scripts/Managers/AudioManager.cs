using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioManager : Singleton<AudioManager>
{
    private EventInstance Music;
    private string currentMusic;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetMusic(string name) 
    {
        if (name == currentMusic)
        {
            return;
        }
        StopMusic();

        currentMusic = name;
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

        currentMusic = "";
    }
    
    public void CharacterTypedSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Typewriter");
    }
    
    public void ContinueTextSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Return");
    }
}
