using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using AnimationTween;

public class MoveStoneDoor : MonoBehaviour
{
    GameObject _player;
    StarterAssetsInputs _input;
    FloatingTipsObject _tip;

    public float InteractDistance;
    public bool IsFrontDoor;
    public int Steps;
    private bool moving;
    private int tap = 0;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _input = FindObjectOfType<StarterAssetsInputs>();
        _tip = GetComponent<FloatingTipsObject>();
    }

    private void Update()
    {
        if(Utilities.DistanceCheck2D(transform, _player.transform) < InteractDistance)
        {
            _tip.InRange = true;
            if(_input.interact && !moving)
            {
                if (tap < Steps)
                {
                    StartCoroutine(MoveRock());
                    AudioManager.PlaySound("RockMove", AudioManager.library, transform.position, false, 1f, 1f);
                }
                else
                {
                    InteractDistance = 0f;
                    return;
                }

                _input.interact = false;
                tap++;
                moving = true;
            }
        }
        else
        {
            _tip.InRange = false;
        }
    }

    private IEnumerator MoveRock()
    {
        float z = transform.localPosition.z;
        float tz;
        if (IsFrontDoor)
        {
            
            tz = z - 0.5f;
        }
        else
        {
            tz = z + 0.5f;
        }
        yield return null;

        float time = 0;
        float duration = 1f;
        while(time < duration)
        {
            float vz = Mathf.Lerp(z, tz, Tween.EaseInOut(time / duration));
            Vector3 pos = new Vector3(transform.localPosition.x, transform.localPosition.y, vz);

            transform.localPosition = pos;
            yield return null;
            time += Time.deltaTime;
            yield return null;
        }

        yield return null;
        moving = false;
    }
}
