using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class CampSiteManager : MonoBehaviour
{
    InputMapManager _mapSwitch;
    GameObject _player;
    StarterAssetsInputs _input;
    FloatingTipsObject _tip;
    WorldRunningMachine _database;

    public float InteractDistance;
    public GameObject SleepingHUDCanvas;

    private bool sleeping;
    private float sleepTime;
    [SerializeField]private GameObject _tiktok;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _mapSwitch = FindObjectOfType<InputMapManager>();
        _input = FindObjectOfType<StarterAssetsInputs>();
        _database = FindObjectOfType<WorldRunningMachine>();
        _tip = GetComponent<FloatingTipsObject>();
    }

    private void Update()
    {
        if (Utilities.DistanceCheck2D(transform, _player.transform) <= InteractDistance)
        {
            _tip.InRange = true;
            if(_input.interact)
            {
                if(_database.CurrentHunger <1f)
                {
                    _input.interact = false;
                    NoticeEvent.current.NoticeTrigger(9);
                }
                else
                {
                    _input.interact = false;
                    _mapSwitch.SwitchMap(6);
                    _mapSwitch.SetCursorState(true);
                    SleepingHUDCanvas.SetActive(true);
                    sleeping = true;
                    sleepTime = 0f;
                    _database.Save();
                    _tiktok = AudioManager.PlayLoop("TimeAccelerate", AudioManager.library);
                    AudioManager.PlaySound("Sleep", AudioManager.library, false, 0.5f);

                    Debug.Log("Let the Wind Down");
                }
            }
            else if (_input.wakeUp)
            {
                _tip.InRange = true;
                AwakeBehaviour();
            }
        }
        else
        {
            _tip.InRange = false;
        }
        if (sleeping)
        {
            _database.Sleep();
            _database.InSleep = true;
            sleepTime += Time.deltaTime;
            if(sleepTime >= 5f || _database.CurrentHunger <= 0)
            {
                sleepTime = 0f;
                AwakeBehaviour();
            }
        }
    }

    private void AwakeBehaviour()
    {
        sleeping = false;
        _mapSwitch.SwitchMap(0);
        _mapSwitch.SetCursorState(true);
        SleepingHUDCanvas.SetActive(false);
        Destroy(_tiktok);
        _database.InSleep = false;
        _input.wakeUp = false;

        Debug.Log("Awake");
    }
}
