using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTipsManager : MonoBehaviour
{
    public static FloatingTipsManager manager;
    public Dictionary<TipType,GameObject> tipsLibrary = new Dictionary<TipType, GameObject>();

    [SerializeField]private TipType[] types;
    [SerializeField]private GameObject[] tips;

    public List<GameObject> TipsPool;

    public Transform Canvas;

    private void Awake()
    {
        if(manager == null)
        {
            manager = this;
        }
        Init();
    }

    private Dictionary<TipType, GameObject> Init()
    {
        if(tips.Length != types.Length) { Debug.Log("Tip and Type Number Unmatched"); return null; }

        for(int i = 0; i <tips.Length; i++)
        {
            tipsLibrary.Add(types[i], tips[i]);
        }

        return tipsLibrary;
    }
}
