using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    GlobalData _database;
    public GameObject Player;
    public InventoryPlayer Inventory;

    

    private void Awake()
    {
        _database = GetComponent<GlobalData>();
    }

    public void SaveToJson()
    {
        string json = JsonUtility.ToJson(_database, true);
        File.WriteAllText(Application.dataPath + "/Save System/Save.json", json);
    }

    public void LoadFromJson(GlobalData target)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Save.json");
        string jsonText = File.ReadAllText(filePath);
        //string json = File.ReadAllText(Application.dataPath + "/Save System/Save.json");
        JsonUtility.FromJsonOverwrite(jsonText,target);
        _database.SaveSystem = this;
    }
}
