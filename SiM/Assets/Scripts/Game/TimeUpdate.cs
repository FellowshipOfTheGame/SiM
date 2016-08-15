using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class TimeUpdate : MonoBehaviour
{
    public string textString = "Tempo:\n";

    private Text textTime;

    void Start()
    {
        textTime = GetComponent<Text>();
    }
    
    void Update()
    {
        textTime.text = "Tempo:\n" + Util.SecondsToString(GameManager.currentTime);
    }
}
