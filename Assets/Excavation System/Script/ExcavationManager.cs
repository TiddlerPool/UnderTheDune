using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class ExcavationManager : MonoBehaviour
{
    LightManager _timeSystem;

    public InventoryExcavationManager Manager;

    public int MaxItemCount;
    public int MinItemCount;

    public StoreRandomItemData[] ItemList;
    public ItemData[] CertainItemList;
    public ItemData[] NewArray;
    public ItemData[] OldArray;
    public ItemData[] ItemsToPassed;

    public bool CertainItem;

    private int itemCount;

    private void Awake()
    {
        _timeSystem = FindObjectOfType<LightManager>();
        itemCount = Random.Range(MinItemCount, MaxItemCount);
    }

    private void Start()
    {

        NewArray = GenerateStoreList();
        OldArray = NewArray;
        ItemsToPassed = NewArray;
    }

    public void PassListToGridManager(bool isCertain, float progress)
    {
        if(progress == -1)
        {
            ItemsToPassed = OldArray;
            Manager.PlaceItemsOnShelf(ItemsToPassed);
        }
        else
        {
            if (isCertain)
            {
                if (CertainItemList.Length == 0)
                {
                    Debug.Log("No preset certain items");
                    return;
                }
                Manager.PlaceItemsOnShelf(CertainItemList);
            }
            else
            {
                GenerateRandomItemList();
                Manager.PlaceItemsOnShelf(ItemsToPassed);
            }
        }
    }

    public void GenerateRandomItemList()
    {
            NewArray = GenerateStoreList();
            ItemsToPassed = NewArray;
            OldArray = NewArray;
    }

    public ItemData[] GenerateStoreList()
    {
        if (ItemList.Length != 0)
        {
            Array.Sort(ItemList, new IntValueComparer());

            NewArray = new ItemData[itemCount];
            for (int i = 0; i < NewArray.Length; i++)
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
            for (int y = 0; y < ItemList.Length; y++)
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

    public ItemData[] DuplicateItemListToStoreList()
    {
        ItemData[] array = new ItemData[Manager.Items.Count];
        for (int i = 0; i < Manager.Items.Count; i++)
        {
            array[i] = Manager.Items[i].ItemData;
        }

        Debug.Log("Item List Copied");
        return array;
    }
}
