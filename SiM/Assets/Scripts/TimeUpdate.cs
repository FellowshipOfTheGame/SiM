using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeUpdate : MonoBehaviour
{
    public Text textTime;

    private int minutes;
    private int seconds;

    // Update is called once per frame
    void Update()
    {
        int currentTime = (int)GameManager.currentTime;
        minutes = currentTime / 60;
        seconds = currentTime % 60;

        textTime.text = "Tempo:\n" + minutes.ToString() + ":" + seconds.ToString("D2");
    }
}
