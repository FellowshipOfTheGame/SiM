using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class Util
{
    public static string SecondsToString(float _seconds)
    {
        string text = "";
        if (_seconds <= 0f)
            return text;
        int seconds = Mathf.FloorToInt(_seconds);
        int hours = 0, minutes = 0;
        if (seconds >= 3600)
        {
            hours = seconds / 3600;
            seconds = seconds - hours * 3600;
            text += hours.ToString() + ":";
        }
        if (seconds >= 60)
        {
            minutes = seconds / 60;
            seconds = seconds - minutes * 60;
            text += minutes.ToString("00") + ":";
        }
        else
            text += "00:";
        text += seconds.ToString("00");
        return text;
    }
}
