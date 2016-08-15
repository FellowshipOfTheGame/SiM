using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class Music : MonoBehaviour
{
    private AudioSource music;
    private static Music instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        music = GetComponent<AudioSource>();
        music.volume = 0f;
    }

    public static void SetVolume(bool on)
    {
        instance.music.volume = on ? 1f : 0f;
    }
}
