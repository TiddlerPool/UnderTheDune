using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemControlTipPressUI : MonoBehaviour
{
    Image icon;
    InventoryController controller;

    public enum Key
    {
        Trade,
        Discard
    }

    public Key _key;
    public float KeyFloat
    {
        get
        {
            if(_key == 0)
            return controller.TradePressTime;
            else
            return controller.DiscardPressTime;
        }
    }

    private Material mat;
    private void Awake()
    {
        icon = GetComponent<Image>();
        controller = FindObjectOfType<InventoryController>();
    }

    private void Start()
    {
        mat = icon.material;
    }

    private void Update()
    {
        mat.SetFloat("_Clip", KeyFloat / 1.5f);
    }
}
