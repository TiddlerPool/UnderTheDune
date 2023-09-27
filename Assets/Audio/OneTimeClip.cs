using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeClip : MonoBehaviour
{
    AudioSource source;
    public int bgmID;
    public bool isMusic;
    public float initVolume;

    private void Start()
    {
        source = GetComponent<AudioSource>();

        if(isMusic)
        {
            AudioEvents.current.onTimeAccelerate += MuteOnAccelerate;
            AudioEvents.current.onMusicVolume += ControlVolume;
        }
        else
        {
            AudioEvents.current.onSoundVolume += ControlVolume;
        }
    }

    public int MuteOnAccelerate()
    {
        Destroy(gameObject);
        return bgmID;  
    }

    public void ControlVolume(float volume)
    {
        source.volume = initVolume * volume;
    }

    private void OnDestroy()
    {
        if (isMusic)
        {
            AudioEvents.current.MusicStopped();
            AudioEvents.current.onMusicVolume -= ControlVolume;
            AudioEvents.current.onTimeAccelerate -= MuteOnAccelerate;
        }
        else
        {
            AudioEvents.current.onSoundVolume -= ControlVolume;
        }
    }
}

