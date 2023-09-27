using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public SaveSystem SaveSystem;
    public WorldRunningMachine WorldMachine;

    public GlobalData Instance
    {
        get;
        private set;
    }

    [Header("Basic States")]
    public float CurrentMaxHealth;
    public float CurrentHealth;
    public float CurrentMaxStamina;
    public float CurrentStamina;
    public float CurrentHunger;
    public float CurrentRaditaion;

    [Header("Mechanism Values")]
    public float Money;
    public float CurrentMaxResearchValue;
    public float CurrentResearchValue;

    [Header("Position Datas")]
    public Vector3 CurrentPosition;
    public Vector3 LastCheckPointPosition;

    [Header("Global Values")]
    public float Times;
    public int Days;

    [Header("Inventory Datas")]
    public List<InventoryItemData> GridItemList_0;
    public List<InventoryItemData> GridItemList_1;
    public List<InventoryItemData> GridItemList_2;
    public List<InventoryItemData> GridItemList_3;

    [Header("Record Datas")]
    public List<RecordObjectData> RecordObjectsData;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SaveSystem = GetComponent<SaveSystem>();
    }


    private void Update()
    {
    }

    public void SaveData()
    {
        SaveSystem.SaveToJson();
    }

    public void LoadData()
    {
        SaveSystem.LoadFromJson(this);
    }

}
