using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using AnimationTween;

public class DamageScreenEffect : MonoBehaviour
{
    Volume volume;
    public float Duration;
    public float Speed;

    public static DamageScreenEffect DamageScreen;

    private void Awake()
    {
        volume = GetComponent<Volume>();
        if(DamageScreen == null)
        {
            DamageScreen = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine("Damage");
        }
    }

    public void DamageEffect()
    {
        StartCoroutine("Damage");
    }

    public IEnumerator Damage()
    {
        
        bool played = false;
        float time = 0f;
        float weight;
        while(time < Duration)
        {
            weight = Mathf.Lerp(0f, 1f, Tween.Spike(time/Duration, 2*Speed, 0.5f, 0.2f));
            if(time/Duration > 0.2 && !played)
            {
                AudioManager.PlaySound("Hurt", AudioManager.library, false);
                played = true;
            }
            volume.weight = weight; 
            time += Time.deltaTime;
            yield return null;
        }
    }
}
