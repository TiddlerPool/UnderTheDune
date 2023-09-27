using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPlayer : InventoryController
{
    WorldRunningMachine _dataSystem;

    public bool Loaded;

    public ItemGrid[] Grids = new ItemGrid[4];
    public List<List<InventoryItemData>> GridItemLists = new List<List<InventoryItemData>>();

    public override void Awake()
    {
        base.Awake();
        _dataSystem = FindObjectOfType<WorldRunningMachine>();
        SaveGridItemsToData();
    }

    private void Start()
    {
        if (_dataSystem == null)
            Debug.Log("Database Not Found");
        
    }

    private void LateUpdate()
    {
        /*
        if(Input.GetKey(KeyCode.K))
            SaveGridItemsToData();

        if (Input.GetKey(KeyCode.L) && !Loaded)
        {
            LoadItemFromSystem();
            Loaded = true;
        }
        */
    }

    public void CloseCanvas()
    {
        canvasTransform.gameObject.SetActive(false);
    }

    public void LoadItemFromSystem()
    {
        PlaceLoadItems();
    }

    public void PlaceLoadItems()
    {
        for (int i = 0; i < GridItemLists.Count; i++)
        {
            if (GridItemLists[i].Count ==0)
            {
                Debug.Log("Grid "+ i +" is Empty, Skiped" );
            }
            else
            {
                foreach (var item in GridItemLists[i])
                {
                    Grids[i].PlaceItem(CreateItemByData(item), item.onGridPositionX, item.onGridPositionY);
                }
            }
        }
    }

    public void SaveGridItemsToData()
    {
        if (GridItemLists == null)
        {
            Debug.Log("GridItemLists Not Found");
            return;
        }

        GridItemLists.Clear();
        for(int i = 0; i < Grids.Length; i++)
        {
            List<InventoryItemData> list = new List<InventoryItemData>();
            foreach(InventoryItem item in Grids[i].GridList)
            { 
                InventoryItemData data = item.Data;
                list.Add(data);
            }
            
            GridItemLists.Add(list);   
        }
        Debug.Log("Grid Items Saved");
    }


}
