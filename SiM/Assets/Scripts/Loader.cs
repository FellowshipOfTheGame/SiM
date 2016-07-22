using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject scoreManager;

    void Awake()
    {
        if (GameManager.GetInstance() == null)
            Instantiate(gameManager);
        if (ScoreManager.GetInstance() == null)
            Instantiate(gameManager);
    }
}
