using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class Loader : MonoBehaviour
{
    public GameObject[] prefabs;
    void Awake()
    {
        foreach (GameObject prefab in prefabs)
        {
            if (prefab != null)
                Instantiate(prefab);
        }
    }
}
