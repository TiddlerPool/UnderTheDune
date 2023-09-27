using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTip : MonoBehaviour
{
    RectTransform trans;
    public RectTransform Dot;
    public RectTransform Tip;
    public Vector3 pos;

    private void Awake()
    {
        trans = GetComponent<RectTransform>();
        Init();
    }

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        Dot.localScale = Vector3.Lerp(Dot.localScale, Vector3.one, Time.deltaTime * 10f);
        Tip.localScale = Vector3.zero;
    }

    private void Update()
    {
        trans.position = pos;

    }

    public void ShowTip()
    {
        Dot.localScale = Vector3.Lerp(Dot.localScale, Vector3.zero, Time.deltaTime * 10f);
        if(Dot.localScale.x < 0.1f)
        {
            Tip.localScale = Vector3.Lerp(Tip.localScale, Vector3.one, Time.deltaTime * 10f);
        }
    }

    public void ZoomTip()
    {
        Tip.localScale = Vector3.Lerp(Tip.localScale, Vector3.zero, Time.deltaTime * 10f);
        if (Tip.localScale.x < 0.1f)
        {
            Dot.localScale = Vector3.Lerp(Dot.localScale, Vector3.one, Time.deltaTime * 10f);
        }
    }

    public void HideTip()
    {
        Dot.localScale = Vector3.Lerp(Dot.localScale, Vector3.zero, Time.deltaTime * 10f);
        Tip.localScale = Vector3.zero;
        if(Dot.localScale.x < 0.1f)
        {
            gameObject.SetActive(false);
        }
    }
}
