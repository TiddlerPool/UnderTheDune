using System;
using UnityEngine;

public class AudioEvents:MonoBehaviour
{
    public static AudioEvents current;
    private void Awake()
    {
        if(current == null)
        {
            current = this;
        }
    }

    public event Action<string> onAudioPlay;
    public void AudioPlay(string name)
    {
        if(onAudioPlay != null)
        {
            onAudioPlay(name);
        }
    }

    public event Action<float> onMusicVolume;
    public void MusicVolume(float volume)
    {
        if (onMusicVolume != null)
        {
            onMusicVolume(volume);
        }
    }

    public event Action<float> onSoundVolume;
    public void SoundVolume(float volume)
    {
        if (onSoundVolume != null)
        {
            onSoundVolume(volume);
        }
    }

    public event Func<int> onTimeAccelerate;
    public void TimeAccelerate()
    {
        if(onTimeAccelerate !=null)
        {
            onTimeAccelerate();
        }
    }

    public event Action onTimeNormal;
    public void TimeNormal()
    {
        if (onTimeNormal != null)
        {
            onTimeNormal();
        }
    }

    public event Action onMusicStopped;
    public void MusicStopped()
    {
        if(onMusicStopped != null)
        {
            onMusicStopped();
        }
    }
}
