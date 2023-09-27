using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightUIUpdate : MonoBehaviour
{
    public Transform pivotPoint;

    [SerializeField]private float offSet;

    private LightManager _manager;
    private RectTransform rectTransform;

    private void Awake()
    {
        _manager = FindObjectOfType<LightManager>();
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        float angleRad = (_manager.TimeOfDay + offSet) /4f * Mathf.Deg2Rad;

        float x = Mathf.Cos(angleRad) * 175f;
        float y = Mathf.Sin(angleRad) * 175f;

        rectTransform.anchoredPosition = new Vector2(x, y) + pivotPoint.GetComponent<RectTransform>().pivot;
    }
}
