using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Pixel : MonoBehaviour
{
    SpriteRenderer sprite;

    public GameManager.SpriteType type;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void OnSelect(Color? color)
    {
        GameManager.Sprites sprites;
        if (color != null)
        {
            sprite.color = color.Value;
            sprites = GameManager.GetInstance().sprites;
        }
        else
        {
            sprite.color = Color.white;
            sprites = GameManager.GetInstance().xSprites;
        }
        switch (type)
        {
            case GameManager.SpriteType.TOP_LEFT:
                sprite.sprite = sprites.topLeft;
                break;
            case GameManager.SpriteType.TOP_RIGHT:
                sprite.sprite = sprites.topRight;
                break;
            case GameManager.SpriteType.BOTTOM_LEFT:
                sprite.sprite = sprites.bottomLeft;
                break;
            case GameManager.SpriteType.BOTTOM_RIGHT:
                sprite.sprite = sprites.bottomRight;
                break;
            case GameManager.SpriteType.CENTER:
            default:
                sprite.sprite = sprites.center;
                break;
        }
        print("ASD");
    }
}
