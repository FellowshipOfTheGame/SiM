using UnityEngine;
using System.Collections;

public class ReturnButton : MonoBehaviour {
    public void OnClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
