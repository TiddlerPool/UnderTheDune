using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DateUIUpdate : MonoBehaviour
{
    private TMP_Text _value;
    private LightManager _timeManager;

    public enum ValueType
    {
        Time,
        Date
    }

    public ValueType Type;

    private void Awake()
    {
        _value = GetComponent<TMP_Text>();
        _timeManager = FindObjectOfType<LightManager>();
    }

    private void Update()
    {
        if (Type.ToString() == "Time")
        {
            _value.text = DateSystem.DateGenerator((int)_timeManager.TimeOfDay, true);
        }
        else
        {
            _value.text = DateSystem.DateGenerator((int)_timeManager.TimeOfDay, _timeManager.Days, 1872, false).Date;
        }

    }
}
