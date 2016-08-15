using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Levels", menuName = "Levels/Create", order = 1)]
public class Collection : ScriptableObject
{
    public Texture2D[] textures;
    public Color[] background;

    int cmp(Texture2D a, Texture2D b)
    {
        return int.Parse(a.name.Substring(0, 2)) - int.Parse(b.name.Substring(0, 2));
    }

    void OnEnable()
    {
        Array.Sort(textures, cmp);
        for (int i = 0; i < background.Length; i++)
        {
            if (background[i].Equals(new Color(0, 0, 0, 0)))
                background[i] = Color.white;
            else if (background[i].a == 0f)
                background[i].a = 1f;
        }
    }
}
