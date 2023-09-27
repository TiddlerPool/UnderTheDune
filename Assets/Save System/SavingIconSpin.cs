using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingIconSpin : MonoBehaviour
{
    public float speed = 300;
    private float num = 0;
    private RectTransform trans;

    private void Awake()
    {
        trans = GetComponent<RectTransform>();
    }

    void Update()
    {
        num -= speed * Time.deltaTime;
        Vector3 rotate = new Vector3(0, 0, num);
        trans.rotation = Quaternion.Euler(rotate);
    }
}
