using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoticeUIManager : MonoBehaviour
{
    public RectTransform NoticeContrainer;
    public GameObject NoticePrefab;
    public string[] Messages;

    private void Start()
    {
        NoticeEvent.current.onNoticeTrigger += AddNewNotice;
    }

    public void AddNewNotice(int id)
    {
        if(id >= Messages.Length)
        {
            Debug.Log("Invalid Notice ID");
            return;
        }

        var notice = Instantiate(NoticePrefab, NoticeContrainer);
        notice.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = Messages[id];
    }
}
