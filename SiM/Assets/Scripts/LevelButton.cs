using UnityEngine;
using System.Collections;

public class LevelButton : MonoBehaviour
{
    public int level;
    public void OnClick()
    {
        BoardGenerator.level = level;
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Game");
    }
}
