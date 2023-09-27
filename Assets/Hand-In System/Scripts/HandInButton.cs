using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInButton : MonoBehaviour
{
    WorldRunningMachine _database;
    HandInManager _manager;
    public ItemGrid MailGrid;

    private void Awake()
    {
        _database = FindObjectOfType<WorldRunningMachine>();
        _manager = GetComponent<HandInManager>();
    }

    public void ProcessToHandIn()
    {
        int num = 0;
        if(MailGrid.GridList.Count == 0)
        {
            NoticeEvent.current.NoticeTrigger(6);
            return;
        }

        for (int i = 0; i< MailGrid.GridList.Count; i++)
        {
            if(MailGrid.GridList[i].ItemID == 3)
            {
                num += 20;
            }

            var type = MailGrid.GridList[i].ItemType;
            if (type != "Evidence")
            {
                NoticeEvent.current.NoticeTrigger(5);
                return;
            }
            else
            {
                num++;
            }
        }

        _database.CurrentMaxResearchValue += num * 5f;
        if(num != 0)
        {
            AudioManager.PlaySound("Research", AudioManager.library, false);
        }
        MailGrid.ClearGrid();
        _manager.CloseBehaviour();
        NoticeEvent.current.NoticeTrigger(7);
    }
}
