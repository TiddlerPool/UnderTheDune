using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkProgressUIUpdate : MonoBehaviour
{
    LightManager _timeSystem;

    public GameObject[] Clocks;

    private void Awake()
    {
        _timeSystem = FindObjectOfType<LightManager>();
    }

    GameObject mainClock;
    private void Update()
    {
        mainClock = Clocks[0];
        if(mainClock.activeSelf)
        {
            _timeSystem.TimeMultiplier = 1f;
        }
        else
        {
            _timeSystem.TimeMultiplier = 300f;
        }

        if (Clocks[1].activeSelf || Clocks[2].activeSelf)
        {
            mainClock.SetActive(false);
        }
        else
        {
            mainClock.SetActive(true);
        }
    }
}
