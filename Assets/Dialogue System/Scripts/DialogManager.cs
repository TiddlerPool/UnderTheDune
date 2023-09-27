using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using StarterAssets;

public class DialogManager : MonoBehaviour
{

#if ENABLE_INPUT_SYSTEM
    private Dialog_Toggle _toggle;
    private DialogStoreManager _storeManager;
    private InventoryController _inventoryController;
    private StarterAssetsInputs _input;
    private InputMapManager _mapManager;

#endif

    [Header("Dialogue Data")]
    public TextAsset DialogueData;
    public Dialog_Character Character;
    public WorldRunningMachine _saveSystem;

    [Header("Dialogue Settings")]
    public float SpeechSpeed;
    public Image CharacterSprite;
    public TMP_Text NameText;
    public TMP_Text DialogueText;
    public Transform OptionsGroup;
    public GameObject InventoryGrid;
    public GameObject StoreGrid;
    Dictionary<string, Sprite> _imageDic = new Dictionary<string, Sprite>();

    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject finishSign;
    [SerializeField] private int dialogueIndex = 0;
    [SerializeField] private int menuIndex;
    private bool isLineFinished;
    [SerializeField]private bool inStore;
    public bool isToggled;
    public string[] DialogueRows;

    private void Awake()
    {
        ReadText(DialogueData);
        _imageDic[Character.Name] = Character.Portrait;
        _toggle = GetComponent<Dialog_Toggle>();
        _storeManager = GetComponent<DialogStoreManager>();
        _input = FindObjectOfType<StarterAssetsInputs>();
        _mapManager = FindObjectOfType<InputMapManager>();
        _inventoryController = FindObjectOfType<InventoryController>();
    }

    public void Toggle()
    {
        isToggled = true;
        UpdateCharacter(Character.Name);
        StartCoroutine(PresentDialogRow());
    }

    private void Update()
    {
        if (!isToggled) { return; }
        _inventoryController.InStore = inStore;

        if (isLineFinished)
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

        StoreInput();
    }

    public void UpdateText(string name, string text)
    {
        NameText.text = name;
        DialogueText.text += text;
    }

    public void UpdateCharacter(string name)
    {
        CharacterSprite.sprite = _imageDic[name];
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
            NameText.text = cells[2];
            dialogueIndex = int.Parse(cells[5]);
            yield return null;
            for (int i = 0; i< cells[4].Length;i++)
            {    
                DialogueText.text += cells[4][i]; 
                yield return new WaitForSecondsRealtime(SpeechSpeed);
            }
            yield return new WaitForSecondsRealtime(0.2f);
            isLineFinished = true;
            yield return null;
        }
        else if(cells[0] == "&" || cells[0] == "$" && int.Parse(cells[1]) == dialogueIndex)
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
        else if (cells[0] == "$")
        {
            GameObject option = Instantiate(buttonPrefab, OptionsGroup);
            option.GetComponentInChildren<TMP_Text>().text = cells[4];
            option.GetComponent<Button>().onClick.AddListener(delegate
            {
                OptionStore(int.Parse(cells[5]));
                AudioManager.PlaySound("Click", AudioManager.library, false);
            });
            GenerateOptions(index + 1);
        }
    }

    private void OptionBranch(string[] cells)
    {
        string cell6Trimmed = cells[6].Trim();
        if (cell6Trimmed == "T")
        {
            if (float.Parse(cells[7])  <= _saveSystem.Money)
            {
                Debug.Log("Approve Trade");
                _saveSystem.Money -= float.Parse(cells[7]);
                _saveSystem.CurrentHealth = _saveSystem.CurrentMaxHealth;
                dialogueIndex = int.Parse(cells[5]);
                AudioManager.PlaySound("Buy", AudioManager.library, false);
            }
            else
            {
                dialogueIndex = int.Parse(cells[3]);
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

    private void OptionStore(int id)
    {
        dialogueIndex = id;
        _input.next = false;
        StartCoroutine(PresentDialogRow());
        for (int i = 0; i < OptionsGroup.childCount; i++)
        {
            Destroy(OptionsGroup.GetChild(i).gameObject);
        }
        Debug.Log("Open Store and Switch Input Map");
        EnableStoreUI(true);
        _storeManager.PassListToGridManager();
        _mapManager.SwitchMap(3);
        _mapManager.SetCursorState(false);
        inStore = true;
    }

    private void StoreInput()
    {
        if(!inStore)
        {
            return;
        }

        if(_input.back)
        {
            inStore = false;
            _input.back = false;
            _mapManager.SwitchMap(1);
            _mapManager.SetCursorState(false);
            EnableStoreUI(false);
            dialogueIndex = menuIndex;
            isLineFinished = false;
            StartCoroutine(PresentDialogRow());
            _storeManager.DuplicateItemListToStoreList();
        }
        else if(_input.leave)
        {
            inStore = false;
            _inventoryController.InStore = inStore;
            _input.leave = false;
            isLineFinished = false;
            EnableStoreUI(false);
            EndDialogBehaviour();
            _storeManager.DuplicateItemListToStoreList();
        }
    }

    private void EnableStoreUI(bool value)
    {
        InventoryGrid.SetActive(value);
        StoreGrid.SetActive(value);
    }
}
