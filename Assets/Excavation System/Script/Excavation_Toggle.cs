using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class Excavation_Toggle : MonoBehaviour
{
    InputMapManager _mapSwitch;
    ExcavationManager _manager;
    GameObject _player;
    GameObject _tiktok;
    GameObject _diging;
    StarterAssetsInputs _input;
    InventoryPlayer _inventory;
    FloatingTipsObject _tip;

    public float InteractDistance;
    public GameObject ExcavationCanvas;
    public GameObject InventoryCanvas;
    public GameObject ProgressHUDCanvas;
    public Image HUDProgress;
    public ItemData[] savedArray;

    [SerializeField]private bool toggled;
    private bool startProgress;
    private float currentProgress = 0f;

    public bool OfferCertainItems;
    public float ProgressDuration;

    private void Awake()
    {
        _manager = GetComponent<ExcavationManager>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _mapSwitch = FindObjectOfType<InputMapManager>();
        _input = FindObjectOfType<StarterAssetsInputs>();
        _inventory = FindObjectOfType<InventoryPlayer>();
        _tip = transform.GetComponent<FloatingTipsObject>();
    }

    private void Update()
    {
       

        if (Utilities.DistanceCheck2D(transform,_player.transform) <= InteractDistance)
        {
            if(!toggled)
            {
                _tip.InRange = true;
                if (currentProgress != -1)
                {
                    if (_input.interact)
                    {
                        _input.interact = false;
                        _mapSwitch.SwitchMap(5);
                        _mapSwitch.SetCursorState(true);
                        startProgress = true;
                        toggled = true;
                        ProgressHUDCanvas.SetActive(true);
                        _inventory.ExcavationToggle = this;
                        _tiktok = AudioManager.PlayLoop("TimeAccelerate", AudioManager.library);
                        _diging = AudioManager.PlayLoop("DigRunning", AudioManager.library);
                        //Debug.Log("Toggled");
                    }
                }
                else
                {
                    if (_input.interact)
                    {
                        toggled = true;
                        _input.interact = false;
                        _mapSwitch.SwitchMap(2);
                        _mapSwitch.SetCursorState(false);
                        ExcavationCanvas.SetActive(true);
                        InventoryCanvas.SetActive(true);
                        ProgressHUDCanvas.SetActive(false);
                        _inventory.ExcavationToggle = this;
                        _manager.PassListToGridManager(OfferCertainItems, currentProgress);
                        AudioManager.PlaySound("BagOpen", AudioManager.library,false);
                    }
                }
            }
            else
            {
                _tip.InRange = false;
                if(_input.stop)
                {
                    _input.stop = false;
                    _mapSwitch.SwitchMap(0);
                    _mapSwitch.SetCursorState(true);
                    toggled = false;
                    _inventory.ExcavationToggle = null;
                    startProgress = false;
                    ProgressHUDCanvas.SetActive(false);
                    Destroy(_tiktok);
                    Destroy(_diging);
                    //Debug.Log("Stopped");
                }
            }
        }
        else
        {
            _tip.InRange = false;
        }

        


        StartProgress();
        ProgressCheck();
    }

    public void HandleClose()
    {
        savedArray = _manager.DuplicateItemListToStoreList();
        _manager.OldArray = savedArray;
        _input.closeInventory = false;
        _inventory.ExcavationToggle = null;
        toggled = false;
        //Debug.Log("Close Search Grid");
    }

    public void StartProgress()
    {
        if (startProgress && toggled)
        {
            currentProgress += Time.deltaTime;
            HUDProgress.fillAmount = currentProgress / ProgressDuration;
        }

    }

    public void ProgressCheck()
    {
        if (currentProgress >= ProgressDuration)
        {
            _input.interact = false;
            _mapSwitch.SwitchMap(2);
            _mapSwitch.SetCursorState(false);
            startProgress = false;
            currentProgress = -1;
            ExcavationCanvas.SetActive(true);
            InventoryCanvas.SetActive(true);
            ProgressHUDCanvas.SetActive(false);
            _manager.PassListToGridManager(OfferCertainItems, currentProgress);
            Destroy(_tiktok);
            Destroy(_diging);
            AudioManager.PlaySound("DigOver", AudioManager.library, false);
            //Debug.Log("Progress Finished");
        }
    }
}
