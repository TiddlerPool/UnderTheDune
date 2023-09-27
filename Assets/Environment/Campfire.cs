using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimationTween;

public class Campfire : MonoBehaviour
{
    LightManager _timeSystem;
    public ParticleSystem Particles;
    public Light Light;
    public bool State;

    private void Awake()
    {
        _timeSystem = FindObjectOfType<LightManager>();
        Particles = GetComponent<ParticleSystem>();
        Light = GetComponentInChildren<Light>();
    }

    private void Update()
    {
        CampfireUpdate();
        State = Particles.isPlaying;
    }

    private void CampfireUpdate()
    {
        if(Utilities.IsBetween<float>(_timeSystem.TimeOfDay,360f,1080f) && Particles.isPlaying)
        {
            Particles.Stop(true);
            StartCoroutine(LightControl(false));
        }
        else if (!Utilities.IsBetween<float>(_timeSystem.TimeOfDay, 360f, 1080f) && !Particles.isPlaying)
        {
            Particles.Play(true);
            StartCoroutine(LightControl(true));
        }
    }

    public IEnumerator LightControl(bool isGrow)
    {
        float time = 0f;
        float duration = 1;
        float intensity;
        if(isGrow)
        {
            while (time < duration)
            {
                intensity = Mathf.Lerp(0f, 1f, Tween.EaseIn(time / duration));
                time += Time.deltaTime;
                Light.intensity = intensity;
                yield return null;
            }
        }
        else
        {
            while (time < duration)
            {
                intensity = Mathf.Lerp(1f, 0f, Tween.EaseOut(time / duration));
                time += Time.deltaTime;
                Light.intensity = intensity;
                yield return null;
            }
        }
    }
}
