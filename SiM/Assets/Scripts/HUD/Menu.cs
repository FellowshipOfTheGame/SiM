using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour
{
    public Collection levels;

    [Range(1, 3)]
    public int collumns = 1;
    public float margin = 25f;

    public GameObject panel;
    public GameObject previous;
    public GameObject next;
    public Text nameText;
    public Sprite lockedLevel;
    public GameObject menuLevelPrefab;

    private GameObject[] menuLevels;
    private int currentPage;
    private GridLayoutGroup gridLayout;
    private RectTransform rectTransform;
    private int levelsPerPage;

    public enum NavType
    {
        Previous,
        Next
    }

    void Start()
    {
        currentPage = 0;
        levelsPerPage = collumns * collumns;

        gridLayout = panel.GetComponent<GridLayoutGroup>();
        rectTransform = panel.GetComponent<RectTransform>();

        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = collumns;

        menuLevels = new GameObject[levelsPerPage];
        for (int i = 0; i < levelsPerPage; i++)
        {
            menuLevels[i] = Instantiate<GameObject>(menuLevelPrefab);
            menuLevels[i].transform.SetParent(panel.transform, false);
        }

        nameText.text = ScoreManager.GetName();

		currentPage = ScoreManager.GetLevel () / (int)(collumns * collumns);
    }

    void Update()
    {
        Rect bounds = rectTransform.rect;
        RectOffset padding = gridLayout.padding;
        Vector2 cellSize = new Vector2();
        float inverseCollumns = 1f / collumns;
        cellSize.x = bounds.width * inverseCollumns - margin;
        cellSize.y = bounds.width * inverseCollumns - margin;

        gridLayout.cellSize = cellSize;
        gridLayout.spacing = new Vector2(margin, margin);
        padding.top = padding.left = Mathf.FloorToInt(margin / 2f);
        gridLayout.padding = padding;

        for (int i = 0; i < levelsPerPage; i++)
        {
            menuLevels[i].SetActive(true);
            int level = i + currentPage * levelsPerPage;
            if (level < ScoreManager.GetLevel())
            {
                Texture2D texture = levels.textures[level];
                int seconds = ScoreManager.GetScore(level);
                menuLevels[i].GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                string text = Util.SecondsToString(seconds);
                if (string.IsNullOrEmpty(text))
                    text = "--:--";
                menuLevels[i].GetComponentInChildren<Text>().text = text;
            }
            else if(level == ScoreManager.GetLevel() && level < levels.textures.Length)
            {
                menuLevels[i].GetComponent<Image>().sprite = lockedLevel;
                menuLevels[i].GetComponentInChildren<Text>().text = "";
            }
            else
                menuLevels[i].SetActive(false);
            menuLevels[i].GetComponent<LevelButton>().level = level;
        }

        CheckMenuNav(previous, NavType.Previous);
        CheckMenuNav(next, NavType.Next);
    }

    private void CheckMenuNav(GameObject button, NavType type)
    {
        switch (type)
        {
            case NavType.Next:
                if (currentPage == Mathf.CeilToInt(ScoreManager.GetLevel() / levelsPerPage + 1) - 1)
                    button.SetActive(false);
                else
                    button.SetActive(true);
                break;
            case NavType.Previous:
                if (currentPage == 0)
                    button.SetActive(false);
                else
                    button.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void Next()
    {
        currentPage++;
    }

    public void Previous()
    {
        currentPage--;
    }
}
