using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeEvent : MonoBehaviour
{
    public static NoticeEvent current;
    private void Awake()
    {
        current = this;
    }

    public event Action<int> onNoticeTrigger;
    public void NoticeTrigger(int id)
    {
        if (onNoticeTrigger != null)
        {
            onNoticeTrigger(id);
            AudioManager.PlaySound("Notice", AudioManager.library, false);
        }
    }

}
