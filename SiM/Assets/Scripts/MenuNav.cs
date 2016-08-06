using UnityEngine;
using System.Collections;

public class MenuNav : MonoBehaviour {
    public enum NavType
    {
        Prev,
        Next
    }
    public Menu menu;
    public NavType type;
    public GameObject other;

    void Start()
    {
        if (menu.CurrentPage == Mathf.CeilToInt(ScoreManager.GetPlayerLevel() + 1) - 1 && type == NavType.Next)
            gameObject.SetActive(false);
        if (menu.CurrentPage == 0 && type == NavType.Prev)
            gameObject.SetActive(false);
    }

    public void OnClick()
    {
        switch (type)
        {
            case NavType.Prev:
                gameObject.SetActive(menu.Prev());
                other.SetActive(true);
                break;
            case NavType.Next:
                gameObject.SetActive(menu.Next());
                other.SetActive(true);
                break;
            default:
                break;
        }
    }
}
