using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class HudController : MonoBehaviour
{
    public GameManager gameManager;
    public Image gameWinBackground;
    public Text gameWinText;

    [Range(0f, 2f)]
    public float delay = 0.5f;
    public string gameWinString = "Voce venceu!";

    private bool hasWon;

    void Start()
    {
        hasWon = false;
    }

    void Update()
    {
        if(GameManager.hasWon && !hasWon)
        {
            hasWon = true;
            StartCoroutine(GameWon());
        }
    }

    IEnumerator GameWon()
    {
        float time = 0f;
        Color c1 = gameWinText.color;
        Color c2 = gameWinBackground.color;
        //string timeString = Util.SecondsToString(GameManager.currentTime);

        c1.a = c2.a = 0f;
        gameWinText.text = gameWinString;// + timeString;
        gameWinBackground.gameObject.SetActive(true);

        do
        {
            c1.a = c2.a = time / delay;
            gameWinText.color = c1;
            gameWinBackground.color = c2;
            yield return null;
            time += Time.deltaTime;
        } while (time < delay);
    }
}
