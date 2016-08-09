using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Loader : MonoBehaviour
{
    public GameObject music;
    public GameObject scoreManager;

    void Awake()
    {
        if (music != null && Music.GetInstance() == null)
            Instantiate(music);
        if (scoreManager != null && ScoreManager.GetInstance() == null)
            Instantiate(scoreManager);
    }
}
