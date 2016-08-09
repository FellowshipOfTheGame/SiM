using UnityEngine;
using System.Collections;

public class ColorChooser : MonoBehaviour
{

    public Color? actualColor;
    SpriteRenderer sprite;
    public Sprite roundSquareSprite;
    public Sprite roundXSprite;
    internal bool selected = false;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void setColor(Color? color)
    {
        if (color == null)
        {
            actualColor = null;
            sprite.sprite = roundXSprite;
            sprite.color = Color.white;
        }
        else
        {
            actualColor = color.Value;
            sprite.sprite = roundSquareSprite;
            sprite.color = color.Value;
        }
    }

    /*
	void OnMouseDown()
	{
		GameManager.GetInstance ().currentColor = actualColor;
		StartCoroutine ("growSize");
	}*/

    IEnumerator growSize()
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

    IEnumerator shrinkSize()
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
