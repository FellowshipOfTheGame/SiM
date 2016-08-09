using UnityEngine;
using UnityEngine.UI;

public class VolumeButton : MonoBehaviour
{
    public Sprite on;
    public Sprite off;

    private bool volume = false;

    public void OnClick()
    {
        volume = !volume;
        if (volume)
            GetComponent<Image>().sprite = on;
        else
            GetComponent<Image>().sprite = off;
        Music.Volume = volume;
    }
}
