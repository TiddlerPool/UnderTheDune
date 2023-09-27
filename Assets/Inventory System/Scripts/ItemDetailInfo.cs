using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemDetailInfo : MonoBehaviour
{
    [Header("Info Panel Settings")]
    public bool InStore;

    public GameObject infoPanel;

    public TMP_Text Name;
    public TMP_Text Type;
    public TMP_Text Price;
    public TMP_Text DetailInfo;
    public GameObject RotateTip;
    public GameObject TradeTip;
    public GameObject UseTip;
    public GameObject DiscardTip;
    public bool Tradable;
    public bool Usable;
    public bool Discardable;

    public void ShowDetailInfo(InventoryItemData data)
    {
        RotateTip.SetActive(false);
        SetUIInfo(data);
        infoPanel.SetActive(true);
    }

    public void ShowDetailInfo(InventoryItemData data, InventoryItem selected)
    {
        if(selected != null)
        {
            RotateTip.SetActive(true);
        }
        else
        {
            RotateTip.SetActive(false);
        }
        SetUIInfo(data);
        infoPanel.SetActive(true);
    }

    public void HideDetailInfo()
    {
        infoPanel.SetActive(false);
    }

    public void SetUIInfo(InventoryItemData data)
    {
        Name.text = data.itemDataName;
        Type.text = data.type;
        Price.text = "$" + data.price.ToString();
        DetailInfo.text = data.detail;
        Tradable = data.tradable;
        Usable = data.usable;
        Discardable = data.discardable;

        if(Tradable && InStore)
        {
            TradeTip.SetActive(true);
        }
        else
        {
            TradeTip.SetActive(false);
        }

        if(UseTip == null) { return; }

        if(Usable && !InStore)
        {
            UseTip.SetActive(true);
        }
        else
        {
            UseTip.SetActive(false);
        }

        if(DiscardTip == null) { return; }

        if(Discardable)
        {
            DiscardTip.SetActive(true);
        }
        else
        {
            DiscardTip.SetActive(false);
        }
    }
}
