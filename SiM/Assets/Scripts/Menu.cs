using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Range(1, 3)]
    public int colsCount = 1;
    public float margin = 25;
    public Collection levels;
    public GameObject panel;
    public GameObject menuLevelPrefab;
    public Text playerName;
    public Sprite locked;

    public int CurrentPage
    {
        get
        {
            return currentPage;
        }
    }

    private GameObject[] menuLevels;
    private int currentPage;
    private GridLayoutGroup gridLayout;
    private RectTransform rectTransform;
    private int levelsPerPage;
    private bool change;

    void Start()
    {
        gridLayout =    panel.GetComponent<GridLayoutGroup>();
        rectTransform = panel.GetComponent<RectTransform>();

        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = colsCount;

        levelsPerPage = colsCount * colsCount;
        menuLevels = new GameObject[levelsPerPage];
        for (int i = 0; i < levelsPerPage; i++)
        {
            menuLevels[i] = Instantiate<GameObject>(menuLevelPrefab);
            menuLevels[i].transform.SetParent(panel.transform, false);
        }

        currentPage = 0;
        change = true;
        playerName.text = ScoreManager.GetPlayerName();
    }

	void Update ()
    {
        if (!change)
            return;
        Rect bounds = rectTransform.rect;
        RectOffset offset = gridLayout.padding;

        gridLayout.cellSize = new Vector2((bounds.width - margin * colsCount)/ colsCount, (bounds.height - margin * colsCount) / colsCount);
        gridLayout.spacing = new Vector2(margin, margin);
        offset.top = offset.left = Mathf.FloorToInt(margin / 2);

        for (int i = 0; i < levelsPerPage; i++)
        {
            menuLevels[i].SetActive(true);
            if (i + currentPage * levelsPerPage < ScoreManager.GetPlayerLevel())
            {
                Texture2D tex = levels.levels[i + currentPage * levelsPerPage].texture;
                int seconds = ScoreManager.GetScore(i + currentPage * levelsPerPage);
                menuLevels[i].GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                if (seconds > 0)
                {
                    string text = "";
                    int hours = 0, minutes = 0;
                    if (seconds >= 3600)
                    {
                        hours = seconds / 3600;
                        seconds = seconds - hours * 3600;
                        text += hours.ToString() + ":";
                    }
                    if (seconds >= 60)
                    {
                        minutes = seconds / 60;
                        seconds = seconds - minutes * 60;
                        text += minutes.ToString("00") + ":";
                    }
                    else
                        text += "00:";
                    if (seconds > 0)
                        text += seconds.ToString("00");
                    menuLevels[i].GetComponentInChildren<Text>().text = text;
                }
                else
                    menuLevels[i].GetComponentInChildren<Text>().text = "--:--";
            }
            else if (i + currentPage * levelsPerPage == ScoreManager.GetPlayerLevel())
            {
                menuLevels[i].GetComponent<Image>().sprite = locked;
                menuLevels[i].GetComponentInChildren<Text>().text = "";
            }
            else
                menuLevels[i].SetActive(false);
            menuLevels[i].GetComponent<LevelButton>().level = i + currentPage * levelsPerPage;
        }
    }

    public bool Next()
    {
        change = true;
        currentPage++;
        if (currentPage == Mathf.CeilToInt(ScoreManager.GetPlayerLevel() + 1) - 1)
            return false;
        return true;
    }

    public bool Prev()
    {
        change = true;
        currentPage--;
        if (currentPage == 0)
            return false;
        return true;
    }
}
