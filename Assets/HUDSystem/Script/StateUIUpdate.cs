using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateUIUpdate : MonoBehaviour
{
    WorldRunningMachine _database;
    public Image HealthBar;
    public Image HungerBar;
    public Image StaminaBar;
    public Image MaxStaminaBar;

    private void Awake()
    {
        _database = GetComponent<WorldRunningMachine>();
    }

    private void Update()
    {
        UpdateStates();
    }

    public void UpdateStates()
    {
        HealthBar.fillAmount = _database.CurrentHealth / _database.MaxHealth;
        HungerBar.fillAmount = _database.CurrentHunger / _database.MaxHunger;
        StaminaBar.fillAmount = _database.CurrentStamina / _database.MaxStamina;
        MaxStaminaBar.fillAmount = _database.CurrentMaxStamina / _database.MaxStamina;
    }
}
