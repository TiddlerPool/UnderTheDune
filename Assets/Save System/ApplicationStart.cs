using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationStart : MonoBehaviour
{
    void Start()
    {
        AudioManager.PlayMusic("DesertDay2", AudioManager.library);
    }
}
