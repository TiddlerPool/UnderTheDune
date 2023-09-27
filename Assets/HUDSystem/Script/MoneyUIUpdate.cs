using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyUIUpdate : MonoBehaviour
{
    public TMP_Text money;
    public WorldRunningMachine database;



    void Update()
    {
        money.text = "$" + database.Money;
    }
}
