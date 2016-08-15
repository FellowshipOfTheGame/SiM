using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class SyncIcon : MonoBehaviour
{
    public Sprite sync;
    public Sprite syncError;
    public float delay = 2f;

    void Start()
    {
        StartCoroutine(SyncLoop());
    }

    IEnumerator SyncLoop()
    {
        while (true)
        {
            if (ScoreManager.Sync())
                GetComponent<Image>().sprite = sync;
            else
                GetComponent<Image>().sprite = syncError;
            yield return new WaitForSeconds(delay);
        }
    }
}
