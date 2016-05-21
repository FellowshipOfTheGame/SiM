using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pixel : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(GameManager.GetInstance().currentColor != null)
            GetComponent<SpriteRenderer>().color = GameManager.GetInstance().currentColor.Value;
    }
}
