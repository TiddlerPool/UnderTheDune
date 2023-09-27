using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using StarterAssets;

public class ExploreManager : MonoBehaviour
{
    private WorldRunningMachine _database;

#if ENABLE_INPUT_SYSTEM
    private Explore_Toggle _toggle;
    private StarterAssetsInputs _input;
    private ResearchRecordManager _manager;
    private LightManager _timeSystem;
#endif

    [Header("Dialogue Data")]
    public TextAsset DialogueData;

    [Header("Dialogue Settings")]
    public float SpeechSpeed;
    public TMP_Text DialogueText;
    public RecordObject RecordObject;
    public Transform OptionsGroup;
    public TMP_FontAsset NormalFont;
    public TMP_FontAsset AncientFont;


    Dictionary<string, Sprite> _imageDic = new Dictionary<string, Sprite>();

    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject finishSign;
    [SerializeField] private int dialogueIndex = 0;

    private bool isLineFinished;
    [SerializeField]private bool recorded;

    public bool isToggled;
    public string[] DialogueRows;

    private void Awake()
    {
        ReadText(DialogueData);
        _toggle = GetComponent<Explore_Toggle>();
        _input = FindObjectOfType<StarterAssetsInputs>();
        _manager = FindObjectOfType<ResearchRecordManager>();
        _timeSystem = FindObjectOfType<LightManager>();
        _database = FindObjectOfType<WorldRunningMachine>();
    }

    public void Toggle()
    {
        isToggled = true;
        StartCoroutine(PresentDialogRow());
    }

    private void Update()
    {
        if (!isToggled) { return; }

        if(isLineFinished)
        {
            if(_input.next)
            {
                _input.next = false;
                isLineFinished = false;
                StartCoroutine(PresentDialogRow());
                AudioManager.PlaySound("Dialog", AudioManager.library, false);
            }
        }
        else
        {
            _input.next = false;
        }

        if(isLineFinished)
        {
            finishSign.SetActive(true);
        }
        else
        {
            finishSign.SetActive(false);
        }

        recorded = _manager.RecordObjectsData[RecordObject.RecordID].isRecorded;
    }

    public void UpdateText(string text)
    {
        DialogueText.text += text;
    }


    public void ReadText(TextAsset data)
    {
        DialogueRows = data.text.Split('\n');
        Debug.Log("Data Read");
    }

    IEnumerator PresentDialogRow()
    {
        var row = DialogueRows[dialogueIndex];
        string[] cells = row.Split('\t');
        
        if (cells[0] == "#")
        {
            DialogueText.text = "";
        }

        if (cells[0] == "#" && int.Parse(cells[1]) == dialogueIndex)
        {
            string content = RecordObject.DetailText;
            if (cells[2] == "Player")
            {
                DialogueText.font = NormalFont;
                DialogueText.fontStyle = FontStyles.Normal;
                dialogueIndex = int.Parse(cells[5]);
                yield return null;
                for (int i = 0; i < cells[4].Length; i++)
                {
                    DialogueText.text += cells[4][i];
                    yield return new WaitForSecondsRealtime(SpeechSpeed);
                }
                yield return new WaitForSecondsRealtime(0.2f);
                isLineFinished = true;
                yield return null;
            }
            else
            {
                DialogueText.font = AncientFont;
                DialogueText.fontStyle = FontStyles.UpperCase;
                dialogueIndex = int.Parse(cells[5]);
                yield return null;
                for (int i = 0; i < content.Length; i++)
                {
                    DialogueText.text += content[i];
                    yield return new WaitForSecondsRealtime(SpeechSpeed);
                }
                yield return new WaitForSecondsRealtime(0.2f);
                isLineFinished = true;
                yield return null;

            }


        }
        else if(cells[0] == "&" && int.Parse(cells[1]) == dialogueIndex)
        {
            GenerateOptions(dialogueIndex);
        }
        else if(cells[0] == "END" && int.Parse(cells[1]) == dialogueIndex)
        {
            isLineFinished = false;
            EndDialogBehaviour();
        }
    }

    private void EndDialogBehaviour()
    {
        Debug.Log("Dialogue Finished");
        isToggled = false;
        dialogueIndex = 0;
        _toggle.FinishDialog();
    }

    public void GenerateOptions(int index)
    {
        string[] cells = DialogueRows[index].Split('\t');
        if (cells[0] == "&")
        {
            GameObject option = Instantiate(buttonPrefab, OptionsGroup);
            option.GetComponentInChildren<TMP_Text>().text = cells[4];
            option.GetComponent<Button>().onClick.AddListener(delegate
            {
                OptionBranch(cells);
                AudioManager.PlaySound("Click", AudioManager.library, false);
            });
            GenerateOptions(index + 1);
        }
    }

    private void OptionBranch(string[] cells)
    {
        string cell6Trimmed = cells[6].Trim();
        Debug.Log(cell6Trimmed);
        if (cell6Trimmed == "T")
        {
            if (recorded)
            {
                Debug.Log("Already Recorded");
                dialogueIndex = int.Parse(cells[3]);
            }
            else
            {
                string DiscoverDay = DateSystem.DateGenerator((int)_timeSystem.TimeOfDay, _timeSystem.Days, 1872, true).Date
                    + DateSystem.DateGenerator((int)_timeSystem.TimeOfDay, _timeSystem.Days, 1872, true).Week;
                Debug.Log("Not Recorded Yet");
                _manager.RecordObjectsData[RecordObject.RecordID].isRecorded = true;
                _manager.RecordObjectsData[RecordObject.RecordID].DiscoverDay = DiscoverDay;
                dialogueIndex = int.Parse(cells[5]);
                NoticeEvent.current.NoticeTrigger(8);
                AudioManager.PlaySound("Research", AudioManager.library, false);
                _database.CurrentMaxResearchValue += 5f;
            }
        }
        else
        {
            dialogueIndex = int.Parse(cells[5]);
        }

        StartCoroutine(PresentDialogRow());
        for (int i = 0; i < OptionsGroup.childCount; i++)
        {
            Destroy(OptionsGroup.GetChild(i).gameObject);
        }
    }

}
