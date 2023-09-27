using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullOffObject : MonoBehaviour
{
    [SerializeField]private Transform targetObject;

    [SerializeField]private LayerMask wallMask;

    private Camera mainCamera;
    private float cullSize;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        Vector2 cutoutPos = mainCamera.WorldToViewportPoint(targetObject.position);
        cutoutPos.y /= (Screen.width / Screen.height);

        Vector3 offset = targetObject.position - transform.position;
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, wallMask);

        for (int i = 0; i < hitObjects.Length; ++i)
        {
            Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;

            for (int m = 0; m < materials.Length; ++m)
            {
                materials[m].SetVector("_CullOutPostion", cutoutPos);
                materials[m].SetFloat("_CullOffSize", 0.15f);
                materials[m].SetFloat("_FallOutSize", 0.05f);
            }
        }
    }
}
