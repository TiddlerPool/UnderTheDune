using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu]
public class RecordObject : ScriptableObject
{ 
    public int RecordID;

    public string Title;
    public string DetailText;
    public Sprite DetailImage;

    public string Note;

    public bool IsImage;
}

