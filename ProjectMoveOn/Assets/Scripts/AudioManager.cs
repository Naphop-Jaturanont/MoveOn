using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound [] sounds;
   
 
    void Awake() 
    {
        foreach(Sound s in sounds)
        { 
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.Loop;

        }
      
    }
    // Start is called before the first frame update
    void Start()
    {
        Play("Theme");
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
        if (s != null)
        {
            Debug.LogWarning("Sound: " + name + " not found!!!");
            return;
        }
        
    }
}
