using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventoryItem : MonoBehaviour
{
    public InventoryItemData Data
    {
        get
        {
            InventoryItemData data = new InventoryItemData();
            data.itemDataID = ItemID;
            data.onGridPositionX = onGridPositionX;
            data.onGridPositionY = onGridPositionY;
            data.itemDataName = ItemData.name;
            data.type = ItemData.Type;
            data.price = ItemData.Price;
            data.useValue = ItemData.UseValue;
            data.detail = ItemData.Detail;
            data.rotated = Rotated;
            data.tradable = ItemData.Tradable;
            data.usable = ItemData.Usable;
            data.discardable = ItemData.Discardable;
            return data;
        }

    }

    public ItemData ItemData;
    public int ItemID;

    public int HEIGHT
    {
        get
        {
            if (Rotated == false)
            {
                return ItemData.height;
            }
            return ItemData.width;
        }
    }

    public int WIDTH
    {
        get
        {
            if (Rotated == false)
            {
                return ItemData.width;
            }
            return ItemData.height; ;
        }
    }

    public int onGridPositionX;
    public int onGridPositionY;
    public Vector2Int GridPositon
    {
        get
        {
            Vector2Int pos = new Vector2Int();
            pos.x = onGridPositionX;
            pos.y = onGridPositionY;
            return pos;
        }
    }

    public float Price;
    public float UseValue;
    public string ItemType;
    public bool Rotated = false;
    public bool Tradable;
    public bool Usable;
    public bool Discardable;

    public void Rotate()
    {
        Rotated = !Rotated;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0, 0, Rotated == true ? 90f : 0f);
    }

    public void Set(ItemData itemData)
    {
        ItemData = itemData;
        Price = itemData.Price;
        UseValue = itemData.UseValue;
        ItemType = itemData.Type;
        ItemID = itemData.ItemID;
        Tradable = itemData.Tradable;
        Usable = itemData.Usable;
        Discardable = itemData.Discardable;


        GetComponent<Image>().sprite = itemData.ItemIcon;

        Vector2 size;
        size.x = itemData.width * ItemGrid.tileSizeWidth;
        size.y = itemData.height * ItemGrid.tileSizeHeight;
        GetComponent<RectTransform>().sizeDelta = size;
    }
}

