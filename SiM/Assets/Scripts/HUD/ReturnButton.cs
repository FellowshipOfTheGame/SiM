using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ReturnButton : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("Menu");
    }
}
