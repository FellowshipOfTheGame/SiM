using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class Line : MonoBehaviour
{
    public Color activeColor;
    public Color defaultColor;
    public Color variantColor;

    [HideInInspector]
    public bool colorVariant;
    [HideInInspector]
    public Vector2 coordinates;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        colorVariant = false;
        spriteRenderer = GetComponent<SpriteRenderer>(); ;
    }

    public void HighLight()
    {
        spriteRenderer.color = activeColor;
    }

    public void LowLight()
    {
        if (colorVariant)
            spriteRenderer.color = variantColor;
        else
            spriteRenderer.color = defaultColor;
    }
}
