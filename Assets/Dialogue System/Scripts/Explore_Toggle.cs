using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Explore_Toggle : MonoBehaviour
{
    InputMapManager _mapSwitch;
    ExploreManager _manager;
    GameObject _player;
    StarterAssetsInputs _input;
    FloatingTipsObject _tip;

    public bool toggled;
    public float InteractDistance;
    public GameObject ExploreCanvas;

    private void Awake()
    {
        _manager = GetComponent<ExploreManager>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _mapSwitch = FindObjectOfType<InputMapManager>();
        _input = FindObjectOfType<StarterAssetsInputs>();
        _tip = GetComponent<FloatingTipsObject>();
    }

    private void Update()
    {
        if (Utilities.DistanceCheck2D(transform, _player.transform) <= InteractDistance)
        {
            if(!toggled)
            {
                _tip.InRange = true;
                if (_input.interact)
                {
                    toggled = true;
                    _input.interact = false;
                    _mapSwitch.SwitchMap(1);
                    _mapSwitch.SetCursorState(false);
                    _manager.Toggle();
                    ExploreCanvas.SetActive(true);
                    AudioManager.PlaySound("Interact", AudioManager.library, false);
                    //Debug.Log("Toggled");
                }
            }
            else
            {
                _tip.InRange = false;
            }
            
        }
        else
        {
            _tip.InRange = false;
        }
    }

    public void FinishDialog()
    {
        toggled = false;
        _input.interact = false;
        _mapSwitch.SwitchMap(0);
        _mapSwitch.SetCursorState(true);
        ExploreCanvas.SetActive(false);
    }
}
