using UnityEngine;
using System.Collections;

public class LevelButton : MonoBehaviour
{
    public int level;
    public void OnClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        // LOAD BOARD
    }
}
