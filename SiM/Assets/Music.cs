using UnityEngine;
using System.Collections;
using System;

public class Music : MonoBehaviour
{
    private static Music instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
        DontDestroyOnLoad(this);
    }

    public static object GetInstance()
    {
        return instance;
    }

    public static bool Volume
    {
        set
        {
            instance.GetComponent<AudioSource>().volume = (value ? 1f : 0f);
        }
    }

}
