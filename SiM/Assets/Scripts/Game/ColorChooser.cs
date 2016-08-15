using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ColorChooser : MonoBehaviour
{
    public Sprite emptySprite;
    public Sprite xSprite;

    [HideInInspector]
    public bool selected;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private Color? _color;
    public Color? color
    {
        get
        {
            return _color;
        }

        set
        {
            if(value == null)
            {
                _color = null;
                spriteRenderer.sprite = xSprite;
                spriteRenderer.color = Color.white;
            }
            else
            {
                _color = value;
                spriteRenderer.sprite = emptySprite;
                spriteRenderer.color = _color.Value;
            }
        }
    }

    IEnumerator Grow()
    {
        selected = true;
        for (float i = 0.8f; i < 0.91f; i += 0.01f)
        {
            if (selected)
            {
                transform.localScale = new Vector3(i, i, 1);
                yield return null;
            }
            else
                break;
        }
    }

    IEnumerator Shrink()
    {
        selected = false;
        for (float i = 0.9f; i > 0.79f; i -= 0.01f)
        {
            if (!selected)
            {
                transform.localScale = new Vector3(i, i, 1);
                yield return null;
            }
            else
                break;
        }
    }
}
