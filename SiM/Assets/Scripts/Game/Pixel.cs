using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class Pixel : MonoBehaviour
{
    public Color? color;
    public Color solution;
    public Color background;

    [HideInInspector]
    public Vector2 coordinates;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(Color solution, Color background)
    {
        this.solution = solution;
        this.background = background;
        this.color = background;
        spriteRenderer.color = background;

        float gamma = (float)(0.2126f * background.r + 0.7152f * background.g + 0.0722 * background.b);
        gamma = (gamma > 0.5f) ? 0f : 1f;
        GetComponentInChildren<TextMesh>().color = new Color(gamma, gamma, gamma);
    }
        
    public void OnSelect(Color? _color)
    {
        TextMesh xText = GetComponentInChildren<TextMesh>();
        if (color.Equals(background))
        {
            if (_color == null)
            {
                color = null;
                spriteRenderer.color = background;
                xText.text = "X";
            }
            else
            {
                color = _color.Value;
                spriteRenderer.color = _color.Value;
                xText.text = "";
            }
        }
        else
        {
            color = background;
            spriteRenderer.color = background;
            xText.text = "";
        }
        StartCoroutine(Wooble());
    }

    public bool IsColorRight()
    {
        return solution.Equals(color.Value);
    }

    IEnumerator Wooble()
    {
        for (float i = 0.9f; i < 1.1f; i += 0.02f)
        {
            transform.localScale = new Vector3(i, i, 1);
            yield return null;
        }
        for (float i = 1f; i > 0.89f; i -= 0.01f)
        {
            transform.localScale = new Vector3(i, i, 1);
            yield return null;
        }
    }

    IEnumerator Fill()
    {
        GetComponentInChildren<TextMesh>().text = "";
        GetComponent<BoxCollider2D>().enabled = false;
        for (float i = .9f; i < 1f; i += 0.01f)
        {
            transform.localScale = new Vector3(i, i, 1);
            yield return null;
        }
    }
}
