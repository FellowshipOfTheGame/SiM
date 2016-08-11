using UnityEngine;
using System.Collections;

public class HudControler : MonoBehaviour
{
	public GameManager gameManager;
	public UnityEngine.UI.Image gameWinBackground;
    public UnityEngine.UI.Text gameWinText;
    public string gameWinString = "Voce venceu!\nTempo: ";
    [Range(0f, 2f)]
    public float delay = 0.5f;

    private bool hasAlredyWon;

    void Start()
    {
        hasAlredyWon = false;
    }

    void Update()
    {
        if(gameManager.hasWon && !hasAlredyWon)
        {
            hasAlredyWon = true;
            StartCoroutine(HasWon());
        }
    }

    IEnumerator HasWon()
    {
        float time = 0.0f;
        Color c1 = gameWinBackground.color;
        Color c2 = gameWinText.color;

        c1.a = c2.a = 0f;
        int score = Mathf.RoundToInt(GameManager.currentTime);
        string timeString = "" + (score / 60) + ":" + (score % 60).ToString("00");
        gameWinText.text = gameWinString + timeString;
        gameWinBackground.gameObject.SetActive(true);

        while (time <= delay)
        {
            yield return null;
            time += Time.deltaTime;
            c1.a = c2.a = time / delay;
            gameWinBackground.color = c1;
            gameWinText.color = c2;
        }
    }
}
