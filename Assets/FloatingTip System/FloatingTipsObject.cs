using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTipsObject : MonoBehaviour
{
    public bool InRange;
    public float TipHeight;
    public TipType Type;
    public GameObject Tip;
    public FloatingTip TipData;

    [SerializeField]private Vector3 pos;

    private void Start()
    {
        CreateTips();
    }

    private void Update()
    {
        pos = Camera.main.WorldToScreenPoint(transform.position);
        VisibleCheck();
        UpdateTipPos();
    }

    private void UpdateTipPos()
    {
        if(TipData == null) { Debug.Log("No Script On Tip"); return; }
        Vector3 worldPos = new Vector3(transform.position.x, transform.position.y + TipHeight, transform.position.z);
        Vector3 tipPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector3 screemPos = new Vector3(tipPos.x, tipPos.y, 0f);
        TipData.pos = screemPos;
    }

    private void CreateTips()
    {
        GameObject frefab = FloatingTipsManager.manager.tipsLibrary[Type];
        Transform trans = FloatingTipsManager.manager.Canvas;
        Tip = Instantiate(frefab, trans);
        TipData = Tip.GetComponent<FloatingTip>();
        FloatingTipsManager.manager.TipsPool.Add(Tip);
    }

    private void VisibleCheck()
    {
        float x = pos.x / Camera.main.pixelWidth;
        float y = pos.y / Camera.main.pixelHeight;

        if (x < 0.8f && y < 0.8f && x> 0.2f && y >0.2f)
        {
            Tip.SetActive(true);
            if(InRange)
            {
                TipData.ShowTip();
            }
            else
            {
                TipData.ZoomTip();
            }
        }
        else
        {
            TipData.HideTip();
        }
    }
}

public enum TipType
{
    Use,
    Talk,
    Move,
    Check,
    Sleep,
    Search
}
