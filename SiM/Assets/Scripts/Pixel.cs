using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Pixel : MonoBehaviour
{
    SpriteRenderer sprite;
    public Color soluctionColor;
    public Color backgroundColor;
    public Color? actualColor;

    internal Vector2 coordinates;


    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void InitializeColors(Color soluction, Color background)
    {
        soluctionColor = soluction;
        backgroundColor = background;
        actualColor = background;
        sprite.color = background;

        float gamma = (float)(0.2126f * background.r + 0.7152f * background.g + 0.0722 * background.b);
        gamma = (gamma > 0.5f) ? 0 : 1;
        GetComponentInChildren<TextMesh>().color = new Color(gamma, gamma, gamma);
    }

    public bool IsColorRight()
    {
        return soluctionColor.Equals(sprite.color);
    }

    IEnumerator wooble()
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

    IEnumerator fill()
    {
        GetComponentInChildren<TextMesh>().text = "";
        GetComponent<BoxCollider2D>().enabled = false;
        for (float i = .9f; i < 1f; i += 0.01f)
        {
            transform.localScale = new Vector3(i, i, 1);
            yield return null;
        }
    }

    public void OnSelect(Color? color)
    {
        StartCoroutine("wooble");
        TextMesh xText = GetComponentInChildren<TextMesh>();
        if (actualColor.Equals(backgroundColor))
        {
            if (color == null)
            {
                actualColor = null;
                sprite.color = backgroundColor;
                xText.text = "X";
            }
            else
            {
                actualColor = color.Value;
                sprite.color = color.Value;
                xText.text = "";
            }
        }
        else
        {
            actualColor = backgroundColor;
            sprite.color = backgroundColor;
            xText.text = "";
        }
    }
}
