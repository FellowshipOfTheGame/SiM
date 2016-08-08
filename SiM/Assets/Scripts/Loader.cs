using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject scoreManager;

    void Awake()
    {
        if (gameManager != null && GameManager.GetInstance() == null)
            Instantiate(gameManager);
        if (scoreManager != null && ScoreManager.GetInstance() == null)
            Instantiate(scoreManager);
    }
}
