using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItemData
{
    public string itemDataName;
    public string type;
    public int itemDataID;
    public int onGridPositionX;
    public int onGridPositionY;
    public string detail;
    public float price;
    public float useValue;
    public bool rotated;
    public bool tradable;
    public bool usable;
    public bool discardable;
}
