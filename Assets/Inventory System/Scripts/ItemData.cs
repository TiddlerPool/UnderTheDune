using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable,CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public int ItemID;

    public int width = 1;
    public int height = 1;

    public float Price;
    public float UseValue;
    public string Type;
    public string Detail;

    public bool Tradable;
    public bool Usable;
    public bool Discardable;

    public Sprite ItemIcon;
}
