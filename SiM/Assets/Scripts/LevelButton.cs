using UnityEngine;
using System.Collections;

public class LevelButton : MonoBehaviour
{
    public int level;
    public void OnClick()
    {
        GameManager.level = level;
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Game");
    }
}
