using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public partial class DialogStoreManager : MonoBehaviour
{
    LightManager _timeSystem;

    public InventoryStoreManager Manager;

    public int MaxItemCount;
    public int MinItemCount;

    public StoreRandomItemData[] ItemList;
    public ItemData[] NewArray;
    public ItemData[] OldArray;
    public ItemData[] ItemsToPassed;

    private int itemCount;
    [SerializeField]private float currentDays;

    private void Awake()
    {
        _timeSystem = FindObjectOfType<LightManager>();
        itemCount = Random.Range(MinItemCount, MaxItemCount);
    }

    private void Start()
    {
        //currentDays = 0;
        NewArray = GenerateStoreList();
        OldArray = NewArray;
        ItemsToPassed = NewArray;
    }

    private void Update()
    {
        RegenerateItemListByTime();
    }

    public void PassListToGridManager()
    {
        Manager.PlaceItemsOnShelf(ItemsToPassed);
    }

    public void RegenerateItemListByTime()
    {
        if(_timeSystem.Days == currentDays)
        {
            ItemsToPassed = OldArray;
        }
        else
        {
            NewArray = GenerateStoreList();
            ItemsToPassed = NewArray;
            OldArray = NewArray;
            currentDays = _timeSystem.Days;
        }
    }

    public ItemData[] GenerateStoreList()
    {
        if (ItemList.Length != 0)
        {
            Array.Sort(ItemList, new IntValueComparer());

            NewArray = new ItemData[itemCount];
            for (int i =0; i< NewArray.Length;i ++)
            {
                DrawnItems(NewArray);
            }
        }

        return NewArray;
    }

    private void DrawnItems(ItemData[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int result = Random.Range(1, 101);
            for (int y =0; y < ItemList.Length; y++)
            {
                if (result < ItemList[y].Chance)
                {
                    array[i] = ItemList[y].data;
                }
            }

            if (array[i] == null)
            {
                array[i] = ItemList[ItemList.Length - 1].data;
            }
            
        }
    }

    public void DuplicateItemListToStoreList()
    {
        ItemData[] array = new ItemData[Manager.Items.Count];   
        for (int i = 0; i < Manager.Items.Count; i++)
        {
            array[i] = Manager.Items[i].ItemData;
        }

        OldArray = array;
    }
}
