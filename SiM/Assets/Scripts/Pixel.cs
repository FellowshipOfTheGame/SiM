using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Pixel : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Color? currentColor;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void OnSelect(Color? color)
    {
        if (color != null)
        {
            sprite.color = color.Value;
            sprite.sprite = GameManager.GetInstance().sprite;
        }
        else
        {
            sprite.color = Color.white;
            sprite.sprite = GameManager.GetInstance().xSprite;
        }

        currentColor = color;
    }
}
