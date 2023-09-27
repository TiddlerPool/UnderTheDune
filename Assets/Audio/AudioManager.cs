using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField]private AudioClip[] clips;
    [SerializeField]private string[] clipNames;
    public static AudioEvents current;
    public Slider[] Sliders;

    private bool enableMaster = true;
    private bool enableMusic = true;
    private bool enableSound = true;

    public bool EnableMaster
    {
        get { return enableMaster; }
        set
        {
            enableMaster = value;
            if (enableMaster)
            {
                masterMulti = masterSlider;
            }
            else
            {
                masterMulti = 0f;
            }
            AudioListener.volume = masterMulti;
        }
    }

    public bool EnableMusic
    {
        get { return enableMusic; }
        set
        {
            enableMusic = value;
            if (enableMusic)
            {
                musicMulti = musicSlider;
            }
            else
            {
                musicMulti = 0f;
            }
            AudioEvents.current.MusicVolume(musicMulti);
        }
    }

    public bool EnableSound
    {
        get { return enableSound; }
        set
        {
            enableSound = value;
            if (enableSound)
            {
                soundMulti = soundSlider;
            }
            else
            {
                soundMulti = 0f;
            }
            AudioEvents.current.SoundVolume(soundMulti);
        }
    }

    [SerializeField] private float masterSlider = 1f;
    [SerializeField] private float musicSlider = 1f;
    [SerializeField] private float soundSlider = 1f;

    public float MasterSlider
    {
        get { return masterSlider; }
        set { masterSlider = value; }
    }

    public float MusicSlider
    {
        get { return musicSlider; }
        set { musicSlider = value; }
    }

    public float SoundSlider
    {
        get { return soundSlider; }
        set { soundSlider = value; }
    }

    public static float masterMulti = 1f;
    public static float musicMulti = 1f;
    public static float soundMulti = 1f;

    public static Dictionary<string, AudioClip> library;


    private void Awake()
    {
        Init();
        if(Sliders.Length == 3)
        {
            Sliders[0].onValueChanged.AddListener(val => ChangeMasterVolume(val));
            Sliders[1].onValueChanged.AddListener(val => ChangeMusicVolume(val));
            Sliders[2].onValueChanged.AddListener(val => ChangeSoundVolume(val));
        }
    }

    private Dictionary<string, AudioClip> Init()
    {
        if(clips.Length != clipNames.Length) { Debug.Log("Name or Clip Unmatched"); return null; }
        library = new Dictionary<string, AudioClip>();
        for(int i = 0; i<clips.Length; i++)
        {
            library.Add(clipNames[i], clips[i]);
        }

        return library;
    }

    public static OneTimeClip PlayMusic(string name, Dictionary<string, AudioClip> library)
    {
        AudioClip clip;
        if(!library.TryGetValue(name, out clip)) { Debug.Log("No Clip Found."); return null; }
        var clipdata = PlayClip(library[name], Vector3.zero, 0.2f, 0f, false, true).GetComponent<OneTimeClip>();
        return clipdata;
    }

    public static GameObject PlayLoop(string name, Dictionary<string, AudioClip> library)
    {
        AudioClip clip;
        if (!library.TryGetValue(name, out clip)) { Debug.Log("No Clip Found."); return null; }
        return PlayClip(library[name], Vector3.zero, 1f ,0f, true,false);
    }

    public static void PlaySound(string name, Dictionary<string, AudioClip> library, bool loop)
    {
        AudioClip clip;
        if (!library.TryGetValue(name, out clip)) { Debug.Log("No Clip Found."); return; }
        PlayClip(library[name], Vector3.zero, 1f, 0f,loop, false);
    }

    public static void PlaySound(string name, Dictionary<string, AudioClip> library, bool loop, float volume)
    {
        AudioClip clip;
        if (!library.TryGetValue(name, out clip)) { Debug.Log("No Clip Found."); return; }
        PlayClip(library[name], Vector3.zero, volume, 0f, loop, false);
    }

    public static void PlaySound(string name, Dictionary<string, AudioClip> library,Vector3 position, bool loop, float volume, float spatial)
    {
        AudioClip clip;
        if (!library.TryGetValue(name, out clip)) { Debug.Log("No Clip Found."); return; }
        PlayClip(library[name], position, volume , spatial, loop, false);
    }

    public static GameObject PlayClip(AudioClip clip, Vector3 position, float volume,float spatial, bool loop, bool music)
    {
        GameObject gameObject = new GameObject("One Time Clip");
        
        gameObject.transform.position = position;
        


        OneTimeClip clipMusic =(OneTimeClip)gameObject.AddComponent(typeof(OneTimeClip));
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        if (music)
        {
            clipMusic.isMusic = true;
            audioSource.volume = volume * musicMulti;
        }
        else
        {
            audioSource.volume = volume * soundMulti;
        }

        if (!loop)
        {
            Destroy(gameObject, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
        }

        clipMusic.initVolume = volume;
        audioSource.clip = clip;
        audioSource.spatialBlend = spatial;
        audioSource.loop = loop;
        audioSource.Play();
        return gameObject;
    }

    public void ChangeMasterVolume(float value)
    {
        masterSlider = value;
        if (enableMaster)
        {
            masterMulti = masterSlider;
        }
        AudioListener.volume = masterMulti;
    }

    public void ChangeMusicVolume(float value)
    {
        musicSlider = value;
        if (enableMusic)
        {
            musicMulti = musicSlider;
        }
        AudioEvents.current.MusicVolume(musicMulti);  
    }

    public void ChangeSoundVolume(float value)
    {
        soundSlider = value;
        if(enableSound)
        {
            soundMulti = soundSlider;
        }
        AudioEvents.current.SoundVolume(soundMulti);
    }

}