using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using StarterAssets;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    StarterAssetsInputs _input;
    InputMapManager _mapManager;

    public GameObject PauseMenu;
    public GameObject PassMenu;
    public GameObject WastedMenu;
    public GameObject TutorialMenu;
    public GameObject SettingMenu;
    public GameObject HUDCanvas;
    public GameObject StateBars;

    private void Awake()
    {
        _input = FindObjectOfType<StarterAssetsInputs>();
        _mapManager = FindObjectOfType<InputMapManager>();
    }

    private void Update()
    {
        if(_input.pause)
        {
            _input.pause = false;
            _mapManager.SwitchMap(7);
            _mapManager.SetCursorState(false);
            PauseMenu.SetActive(true);
            HUDCanvas.SetActive(false);
            StateBars.SetActive(false);
            AudioManager.PlaySound("Pause", AudioManager.library, false, 0.2f);
        }
        else if(_input.endPause)
        {
            Resume();
        }
    }



    public void PassGame()
    {
        _input.pause = false;
        _mapManager.SwitchMap(7);
        _mapManager.SetCursorState(false);
        PassMenu.SetActive(true);
        HUDCanvas.SetActive(false);
        StateBars.SetActive(false);
    }

    public void LostGame()
    {
        _input.pause = false;
        _mapManager.SwitchMap(7);
        _mapManager.SetCursorState(false);
        WastedMenu.SetActive(true);
        HUDCanvas.SetActive(false);
        StateBars.SetActive(false);
    }

    public void Resume()
    {
        _input.endPause = false;
        _mapManager.SwitchMap(0);
        _mapManager.SetCursorState(false);
        PauseMenu.SetActive(false);
        TutorialMenu.SetActive(false);
        SettingMenu.SetActive(false);
        HUDCanvas.SetActive(true);
        StateBars.SetActive(true);
        AudioManager.PlaySound("Pause", AudioManager.library, false, 0.2f);
    }

    public void Restart()
    {
        var singleton = FindObjectOfType<GlobalData>().gameObject;
        Destroy(singleton);
        SceneManager.LoadScene(1);
    }
}
