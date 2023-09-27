using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public ItemData ItemData;

    public int HEIGHT
    {
        get
        {
            if(rotated == false)
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
            if(rotated == false )
            {
                return ItemData.width;
            }
            return ItemData.height; ;
        }
    }

    public int onGridPositionX;
    public int onGridPositionY;

    public bool rotated = false;

    public void Rotate()
    {
        rotated = !rotated;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0, 0, rotated == true ? 90f : 0f);
    }

    public void Set(ItemData itemData)
    {
        ItemData = itemData;

        GetComponent<Image>().sprite = itemData.ItemIcon;

        Vector2 size;
        size.x = itemData.width * ItemGrid.tileSizeWidth;
        size.y = itemData.height * ItemGrid.tileSizeHeight;
        GetComponent<RectTransform>().sizeDelta = size;
    }
}
