using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SyncIcon : MonoBehaviour
{
    public float delay = 2f;
    public Sprite ok;
    public Sprite problem;
    void Start()
    {
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        while (true)
        {
            if (ScoreManager.Sync())
                GetComponent<Image>().sprite = ok;
            else
                GetComponent<Image>().sprite = problem;
            yield return new WaitForSeconds(delay);
        }
    }
}
