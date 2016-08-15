using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class LevelButton : MonoBehaviour
{
    public int level;

    public void OnClick()
    {
        BoardGenerator.level = level;
        SceneManager.LoadScene("Game");
    }
}
