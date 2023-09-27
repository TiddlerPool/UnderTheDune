using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldRunningMachine: MonoBehaviour
{
    public GlobalData DataController;
    public LightManager TimeSyestem;
    public TMP_Text RestartButton;
    public GameObject Player;
    public StarterAssetsInputs InputAsset;
    public ThirdPersonController PlayerController;
    public InventoryPlayer Inventory;
    public ResearchRecordManager Record;
    public MenuManager Menu;

    [Header("Basic States")]
    public float MaxHealth = 100f;
    public float CurrentMaxHealth;
    public float CurrentHealth;
    public float MaxStamina = 100f;
    public float CurrentMaxStamina;
    public float CurrentStamina;
    public float MaxHunger = 100f;
    public float CurrentHunger;
    public float MaxRadiation = 1000f;
    public float CurrentRaditaion;

    [Header("Mechanism Values")]
    public float Money;
    public float MaxResearchValue = 100f;
    public float CurrentMaxResearchValue;
    public float CurrentResearchValue;

    [Header("Position Datas")]
    public Vector3 CurrentPosition;
    public Vector3 LastCheckPointPosition;

    [Header("Global Values")]
    public float Times;
    public int Days;

    [Header("Inventory Datas")]
    public List<InventoryItemData> GridItemList_0;
    public List<InventoryItemData> GridItemList_1;
    public List<InventoryItemData> GridItemList_2;
    public List<InventoryItemData> GridItemList_3;

    [Header("Record Datas")]
    public List<RecordObjectData> RecordObjectsData;

    public bool IsDead;
    public bool InSleep;
    public bool InRadiation;
    public bool IsStoryMode;
    public bool ResearchStopped;

    private void Awake()
    {
        DataController = FindObjectOfType<GlobalData>();
    }

    private void Start()
    {
        Load();
    }

    private void Update()
    {
        if(IsDead)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Load();

        }

        StateInteract();
        StateValueClamp();
        PassCheck();
        WastedCheck();
    }

    public void Save()
    {
        SaveInventory();
        SaveRecord();
        SaveStateToData(DataController);
        SaveProgressValueToData(DataController);
        SaveInventoryToData(DataController);
        SaveRecordToData(DataController);
        SavePositionData(DataController);
    }

    public void Load()
    {
        DataController.LoadData();

        LoadPositionData(DataController);
        LoadStateFromData(DataController);
        LoadProgressValueFromData(DataController);
        LoadInventoryFromData(DataController);
        LoadRecordFromData(DataController);
        LoadInventory();
        LoadRecord();
    }

    private void SavePositionData(GlobalData data)
    {
        data.CurrentPosition = CurrentPosition;
    }

    private void LoadPositionData(GlobalData data)
    {
        CurrentPosition = data.CurrentPosition;
    }

    private void SaveStateToData(GlobalData data)
    {
        data.CurrentMaxHealth = CurrentMaxHealth;
        data.CurrentHealth = CurrentHealth;
        data.CurrentHunger = CurrentHunger;
        data.CurrentMaxStamina = CurrentMaxStamina;
        data.CurrentStamina = CurrentStamina;
        data.CurrentRaditaion = CurrentRaditaion;
    }

    private void LoadStateFromData(GlobalData data)
    {
        CurrentMaxHealth = data.CurrentMaxHealth;
        CurrentHealth = data.CurrentHealth;
        CurrentHunger = data.CurrentHunger;
        CurrentMaxStamina = data.CurrentMaxStamina;
        CurrentStamina = data.CurrentStamina;
        CurrentRaditaion = data.CurrentRaditaion;
    }

    private void SaveProgressValueToData(GlobalData data)
    {
        data.Times = Times;
        data.Days = Days;
        data.Money = Money;
        data.CurrentMaxResearchValue = CurrentMaxResearchValue;
        data.CurrentResearchValue = CurrentResearchValue;
    }

    private void LoadProgressValueFromData(GlobalData data)
    {
        Times = data.Times;
        Days = data.Days;
        Money = data.Money;
        CurrentMaxResearchValue = data.CurrentMaxResearchValue;
        CurrentResearchValue = data.CurrentResearchValue;
    }

    private void SaveInventoryToData(GlobalData data)
    {

        data.GridItemList_0 = GridItemList_0;
        data.GridItemList_1 = GridItemList_1;
        data.GridItemList_2 = GridItemList_2;
        data.GridItemList_3 = GridItemList_3;
        Debug.Log("Inventory Save Sucessed");
    }

    private void LoadInventoryFromData(GlobalData data)
    {
        GridItemList_0 = data.GridItemList_0;
        GridItemList_1 = data.GridItemList_1;
        GridItemList_2 = data.GridItemList_2;
        GridItemList_3 = data.GridItemList_3;
        Debug.Log("Inventory Load Sucessed");


    }

    private void SaveRecordToData(GlobalData data)
    {
        data.RecordObjectsData = RecordObjectsData;
    }

    private void LoadRecordFromData(GlobalData data)
    {
        RecordObjectsData = data.RecordObjectsData;
    }

    private void SaveInventory()
    {
        if (Inventory.GridItemLists.Count == 0) { Debug.Log("Save Failed"); return; }
        GridItemList_0 = Inventory.GridItemLists[0];
        GridItemList_1 = Inventory.GridItemLists[1];
        GridItemList_2 = Inventory.GridItemLists[2];
        GridItemList_3 = Inventory.GridItemLists[3];
        Debug.Log("Inventory Save Sucessed");

        Inventory.SaveGridItemsToData();
    }

    private void LoadInventory()
    {
        if (Inventory.GridItemLists.Count == 0) { Debug.Log("Load Failed"); return; }
        Inventory.GridItemLists[0] = GridItemList_0;
        Inventory.GridItemLists[1] = GridItemList_1;
        Inventory.GridItemLists[2] = GridItemList_2;
        Inventory.GridItemLists[3] = GridItemList_3;
        Debug.Log("Inventory Load Sucessed");
        Inventory.LoadItemFromSystem();
        Inventory.CloseCanvas();
    }

    private void SaveRecord()
    {
        RecordObjectsData = Record.RecordObjectsData;
    }

    private void LoadRecord()
    {
        Record.RecordObjectsData = RecordObjectsData;
    }

    public void StateValueClamp()
    {
        CurrentMaxHealth = Mathf.Clamp(CurrentMaxHealth, 0f, MaxHealth);
        CurrentMaxStamina = Mathf.Clamp(CurrentMaxStamina, 10f, MaxStamina);
        CurrentMaxResearchValue = Mathf.Clamp(CurrentMaxResearchValue, 0f, MaxResearchValue);
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        CurrentHunger = Mathf.Clamp(CurrentHunger, 0f, MaxHunger);
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0f, CurrentMaxStamina);
        CurrentRaditaion = Mathf.Clamp(CurrentRaditaion, 0f, MaxRadiation);
    }

    public void SyncTime()
    {
        Times = TimeSyestem.TimeOfDay;
        Days = TimeSyestem.Days;
    }

    public void LoadTime()
    {
        TimeSyestem.TimeOfDay = Times;
    }


    bool hungerNote;
    bool sleepNote;
    bool radiNote1;
    bool radiNote2;
    public void StateInteract()
    {
        CurrentPosition = Player.transform.position;


        if (CurrentResearchValue >= CurrentMaxResearchValue)
        {
            ResearchStopped = true;
        }
        else
        {
            ResearchStopped = false;
            if (CurrentMaxResearchValue >= 100f)
            {
                CurrentResearchValue += Time.deltaTime * 6f * TimeSyestem.TimeMultiplier;
            }
            else
            {
                CurrentResearchValue += Time.deltaTime / 2f * TimeSyestem.TimeMultiplier;
            }

        }

        if (IsStoryMode) { return; }

        if (CurrentStamina >= 0.1f)
        {
            InputAsset.allowSprint = true;
            if (InputAsset.sprint && PlayerController.CurrentSpeed > PlayerController.MoveSpeed)
            {
                CurrentStamina -= 6f * Time.deltaTime * TimeSyestem.TimeMultiplier;
            }
            else
            {
                float speed;
                speed = Mathf.Lerp(1f, 10f, CurrentHunger / MaxHunger);
                CurrentStamina += speed * Time.deltaTime * TimeSyestem.TimeMultiplier;
            }
        }
        else
        {
            InputAsset.allowSprint = false;
            InputAsset.sprint = false;
            float speed;
            speed = Mathf.Lerp(1f, 10f, CurrentHunger / MaxHunger);
            CurrentStamina += speed * Time.deltaTime * TimeSyestem.TimeMultiplier;
        }

        if(InRadiation)
        {
            CurrentRaditaion += 2f;
            PlayerController.InRadiation = true;
        }
        else
        {
            CurrentRaditaion --;
            PlayerController.InRadiation = false;
        }

        if(!InSleep)
        {
            CurrentHunger -= 0.1f * Time.deltaTime * TimeSyestem.TimeMultiplier;
            if (CurrentHunger <= 50f)
            {
                CurrentMaxStamina -= 0.1f * Time.deltaTime * TimeSyestem.TimeMultiplier;
            }
            else
            {
                CurrentMaxStamina -= 0.03f * Time.deltaTime * TimeSyestem.TimeMultiplier;
            }
        }
        else
        {
            CurrentHunger -= 0.03f * Time.deltaTime * TimeSyestem.TimeMultiplier;
        }

        if(CurrentRaditaion > 400f && !radiNote1)
        {
            NoticeEvent.current.NoticeTrigger(1);
            radiNote1 = true;
        }
        if(CurrentRaditaion > 700f && !radiNote2)
        {
            NoticeEvent.current.NoticeTrigger(2);
            radiNote2 = true;
        }

        if(CurrentRaditaion <= 400f)
        {
            radiNote1 = false;
        }

        if (CurrentRaditaion <= 700f)
        {
            radiNote2 = false;
        }

        if (CurrentHunger <= 0f && !hungerNote)
        {
            NoticeEvent.current.NoticeTrigger(3);
            hungerNote = true;
        }
        else if(CurrentHunger > 0f)
        {
            hungerNote = false;
        }

        if (CurrentMaxStamina <= 20f && !sleepNote)
        {
            NoticeEvent.current.NoticeTrigger(4);
            sleepNote = true;
        }
        else if (CurrentMaxStamina > 20f)
        {
            sleepNote = false;
        }

        if (CurrentHunger <= 0f || CurrentRaditaion >= 900f) 
        {
            StarveToHurt();
        }
        //Debug.Log("Save System is working");
    }

    float sth_time = 0f;
    float sth_duration = 5f;
    bool sth_start = true;
    bool effected = false;
    public void StarveToHurt()
    {
        if (sth_time < sth_duration)
        {
            sth_start = true;
        }

        if (sth_start)
        {
            sth_time += Time.deltaTime;
            if (sth_time >= sth_duration - 0.3f)
            {
                if(!effected)
                {
                    DamageScreenEffect.DamageScreen.DamageEffect();
                    effected = true;
                }
                
                if(sth_time >= sth_duration)
                {
                    effected = false;
                    sth_start = false;
                    CurrentHealth -= 10f;
                    sth_time = 0f;
                }

            }
        }

    }

    public void Sleep()
    {
        if (CurrentMaxStamina < MaxStamina)
        {
            CurrentMaxStamina += 20f * Time.deltaTime;
        }
    }

    public void SetStoryMode()
    {
        IsStoryMode = !IsStoryMode;
        SwapText(RestartButton);
    }

    private void SwapText(TMP_Text btn)
    {
        string state;
        if (IsStoryMode)
        {
            state = "On";
            StoryMode();
        }
        else
        {
            state = "Off";
        }

        btn.text = string.Format("StoryMode: {0}", state);
    }

    bool gamePassed;
    public void PassCheck()
    {
        if(CurrentResearchValue >= 100f && !gamePassed)
        {
            gamePassed = false;
            Menu.PassGame();
        }
    }

    bool gameWasted;
    public void WastedCheck()
    {
        if (CurrentHealth <= 0f && !gameWasted)
        {
            IsDead = true;
            gameWasted = false;
            Menu.LostGame();
        }
    }

    private void StoryMode()
    {
        CurrentMaxHealth = 100f;
        CurrentHealth = 100f;
        CurrentMaxStamina = 100f;
        CurrentStamina = 100f;
        CurrentHunger = 100f;
        CurrentRaditaion = 0f;
        Money = 99900f;
    }
}
