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
