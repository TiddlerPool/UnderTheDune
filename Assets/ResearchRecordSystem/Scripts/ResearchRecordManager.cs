using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using StarterAssets;

public class ResearchRecordManager : MonoBehaviour
{
#if ENABLE_INPUT_SYSTEM
    private StarterAssetsInputs _input;
    private InputMapManager _mapManager;
#endif


    public GameObject RecordCanvas;
    public GameObject ListContainer;
    public GameObject BookContent;

    public Image DetailImage;
    public TMP_Text DetailText;
    public TMP_Text DiscoverDate;
    public TMP_Text NoteDetail;

    public GameObject ButtonPrefab;
    public RecordObject[] RecordLibrary;
    public List<RecordObjectData> RecordObjectsData;

    private void Awake()
    {
        _input = FindObjectOfType<StarterAssetsInputs>();
        _mapManager = FindObjectOfType<InputMapManager>();
    }

    private void Start()
    {
        Array.Sort(RecordLibrary, new RecordIDComparer());
        //CopyRecordObjectToData();
    }

    private void Update()
    {
        CanvasToggle();
    }
    
    private void CanvasToggle()
    {
        if (_input.note)
        {
            _input.note = false;
            _mapManager.SwitchMap(4);
            _mapManager.SetCursorState(false);
            LoadRecordedObjcetList();
            HideOrShowBookContent(false);
            RecordCanvas.SetActive(true);
            AudioManager.PlaySound("BookOpen", AudioManager.library, false);
        }
        else if(_input.closeNote)
        {
            _input.closeNote = false;
            _mapManager.SwitchMap(0);
            _mapManager.SetCursorState(true);
            RecordCanvas.SetActive(false);
            AudioManager.PlaySound("BookClose", AudioManager.library, false);
        }  
    }

    public RecordObjectData GetObjectToData(int id, string discoverDate)
    {
        RecordObjectData data = new RecordObjectData();
        data.RecordID = RecordLibrary[id].RecordID;
        data.DiscoverDay = discoverDate;
        return data;
    }

    private void LoadRecordedObjcetList()
    {
        foreach(Transform child in ListContainer.transform)
        {
            Destroy(child.gameObject);
        }

        for(int i =0; i< RecordObjectsData.Count; i++)
        {
            int recordIndex = i;
            if (RecordObjectsData[recordIndex].isRecorded)
            {
                var obj = Instantiate(ButtonPrefab, ListContainer.transform);
                obj.GetComponent<Button>().onClick.AddListener(delegate
                {
                    UpdateRecordBook(RecordObjectsData[recordIndex]);
                    AudioManager.PlaySound("BookClick", AudioManager.library, false);
                });
                var text = obj.GetComponentInChildren<TMP_Text>();
                text.text = RecordLibrary[recordIndex].Title;
            }
        }
    }

    public void UpdateRecordBook(RecordObjectData data)
    {
        if (!RecordLibrary[data.RecordID].IsImage)
        {
            DetailImage.sprite = null;
            DetailImage.gameObject.SetActive(false);
            DetailText.text = RecordLibrary[data.RecordID].DetailText;
            DiscoverDate.text = data.DiscoverDay;
            NoteDetail.text = RecordLibrary[data.RecordID].Note;
        }
        else
        {
            DetailImage.sprite = RecordLibrary[data.RecordID].DetailImage;
            DetailImage.gameObject.SetActive(true);
            DetailText.text = "";
            DiscoverDate.text = data.DiscoverDay;
            NoteDetail.text = RecordLibrary[data.RecordID].Note;
        }
        HideOrShowBookContent(true);
    }

    private void HideOrShowBookContent(bool value)
    {
        BookContent.SetActive(value);
    }

    private void CopyRecordObjectToData()
    {
        //RecordObjectsData.Clear();

        for(int i = 0; i < RecordLibrary.Length; i++)
        {
            RecordObjectsData.Add(GetObjectToData(RecordLibrary[i].RecordID, ""));
        }
        if(RecordObjectsData.Count < RecordLibrary.Length)
        {
            CopyRecordObjectToData();
        }
    }
}
