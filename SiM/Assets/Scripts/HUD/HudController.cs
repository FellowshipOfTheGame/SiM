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
    public Text levelNumberText;
    public Text gameWinText;
    public Image gameWinBackground;

    [Range(0f, 2f)]
    public float delay = 0.5f;

    private bool hasWon;

    void Start()
    {
        hasWon = false;
        levelNumberText.text = (BoardGenerator.level + 1).ToString("00");
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

        c1.a = c2.a = 0f;
        gameWinBackground.gameObject.SetActive(true);

        do
        {
            c1.a = c2.a = time / delay;
            gameWinText.color = c1;
            gameWinBackground.color = c2;
            yield return null;
            time += Time.deltaTime;
        } while (time <= delay);

        c1.a = c2.a = 1f;
        gameWinText.color = c1;
        gameWinBackground.color = c2;

		time = 2f;
		while (time > 0) {
			time -= Time.deltaTime;
			yield return null;
		}
		SceneManager.LoadScene ("Menu");
    }
}
