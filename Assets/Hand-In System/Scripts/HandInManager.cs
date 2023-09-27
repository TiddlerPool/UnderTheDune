using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class HandInManager : MonoBehaviour
{
    InputMapManager _mapSwitch;
    GameObject _player;
    StarterAssetsInputs _input;
    FloatingTipsObject _tip;

    public float InteractDistance;
    public GameObject HandInCanvas;
    public GameObject InventoryCanvas;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _mapSwitch = FindObjectOfType<InputMapManager>();
        _input = FindObjectOfType<StarterAssetsInputs>();
        _tip = GetComponent<FloatingTipsObject>();
    }


    private void Update()
    {
        if (Utilities.DistanceCheck2D(transform, _player.transform) <= InteractDistance)
        {
            _tip.InRange = true;
            if(_input.interact)
            {
                _input.interact = false;
                _mapSwitch.SwitchMap(2);
                _mapSwitch.SetCursorState(false);
                HandInCanvas.SetActive(true);
                InventoryCanvas.SetActive(true);
                AudioManager.PlaySound("BagOpen", AudioManager.library, false);
            }
        }
        else
        {
            _tip.InRange = false;
        }
    }

    public void CloseBehaviour()
    {
        _input.closeInventory = false;
        _mapSwitch.SwitchMap(0);
        _mapSwitch.SetCursorState(true);
        HandInCanvas.SetActive(false);
        InventoryCanvas.SetActive(false);
    }
}
