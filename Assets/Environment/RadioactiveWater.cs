using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioactiveWater : MonoBehaviour
{
    WorldRunningMachine _database;

    private void Awake()
    {
        _database = FindObjectOfType<WorldRunningMachine>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        _database.InRadiation = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            _database.InRadiation = false;
    }
}
