using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResearchUIUpdate : MonoBehaviour
{
    WorldRunningMachine _database;

    public TMP_Text ProgressNumber;
    public TMP_Text ProgressState;
    public GameObject StateIcon;

    private void Awake()
    {
        _database = FindObjectOfType<WorldRunningMachine>();
    }

    private void Update()
    {
        int num = (int)_database.CurrentResearchValue;
        ProgressNumber.text = num.ToString();

        if(_database.ResearchStopped)
        {
            StateIcon.SetActive(true);
            ProgressState.text = "Research\nStopped";
        }
        else
        {
            StateIcon.SetActive(false);
            ProgressState.text = "Research\nProgress";
        }
    }
}
