using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecordButton : MonoBehaviour
{
    public RecordObjectData RecordData;
    ResearchRecordManager _manager;
    RecordObject RecordObj;

    private void Awake()
    {
        _manager = FindObjectOfType<ResearchRecordManager>();
        RecordObj = _manager.RecordLibrary[RecordData.RecordID];
    }


}
