using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class VolumeButton : MonoBehaviour
{
    public Sprite spriteOn;
    public Sprite spriteOff;
    private bool on;

    void Start()
    {
        on = false;
        Music.SetVolume(on);
        GetComponent<Image>().sprite = spriteOff;
    }

    public void Swap()
    {
        on = !on;
        if(on)
            GetComponent<Image>().sprite = spriteOn;
        else
            GetComponent<Image>().sprite = spriteOff;
        Music.SetVolume(on);
    }
}
