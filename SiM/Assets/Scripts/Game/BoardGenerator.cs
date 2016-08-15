using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class BoardGenerator : MonoBehaviour
{
    public static int level = -1;

    [System.Serializable]
    public struct Sprites
    {
        public Sprite center;
        public Sprite topLeft;
        public Sprite topRight;
        public Sprite bottomLeft;
        public Sprite bottomRight;
    }

    public Sprites sprites;
    public Collection levels;
    public Color colorChooserBackground = new Color(0.7382f, 0.7382f, 0.7382f);

    public GameObject iconPrefab;
    public GameObject linePrefab;
    public GameObject pixelPrefab;
    public GameObject tipSquarePrefab;
    public GameObject colorChooserPrefab;

    private Color[,] board;
    private GameObject boardObject;
    private GameObject colorBoardObject;

    void Start()
    {
        board = null;
        boardObject = null;
        colorBoardObject = null;
    }

    public void LoadBoard()
    {
        if (level == -1)
            return;

        Texture2D map = levels.textures[level];
        Color backgroundColor = levels.background[level];

        Color[] inlineBoard = map.GetPixels();
        List<Color?> availableColors = new List<Color?>();

        #region Board

        board = new Color[map.width, map.height];
        boardObject = new GameObject("Board");

        availableColors.Add(backgroundColor);
        availableColors.Add(null);


        GameObject line = Instantiate(linePrefab);
        line.GetComponent<Line>().coordinates = new Vector2(-1, -1);

        line.transform.SetParent(boardObject.transform);
        line.transform.localScale = new Vector3(map.width, map.height, 1);
        line.transform.localPosition = new Vector2(map.width / 2, map.height / 2);


        for (int i = 0; i < map.height; i++)
        {
            for (int j = 0; j < map.width; j++)
            {
                board[j, i] = inlineBoard[i * map.width + j];
                GameObject spriteObject = Instantiate(pixelPrefab);
                spriteObject.transform.SetParent(boardObject.transform);
                spriteObject.transform.localPosition = new Vector2(j, i);

                spriteObject.GetComponent<Pixel>().coordinates = new Vector2(j, i);
                spriteObject.GetComponent<Pixel>().Init(board[j, i], backgroundColor);

                if (!availableColors.Contains(board[j, i]))
                    availableColors.Add(board[j, i]);
            }
        }

        #endregion

        #region Color Picker

        colorBoardObject = new GameObject("Color Picker");

        int colorWidth = (availableColors.Count - 2 > 3) ? 4 : 3;
        int colorCounter = 0;

        GameManager.currentColor = availableColors[2];

        for (int h = 1; h >= 0; h--)
        {
            for (int w = 0; w < colorWidth; w++)
            {

                GameObject background = Instantiate(iconPrefab);
                if (w == 0)
                {
                    if (h == 1)
                        background.GetComponent<SpriteRenderer>().sprite = sprites.topLeft;
                    else if (h == 0)
                        background.GetComponent<SpriteRenderer>().sprite = sprites.bottomLeft;
                    else
                        background.GetComponent<SpriteRenderer>().sprite = sprites.center;
                }
                else if (w == (colorWidth - 1))
                {
                    if (h == 1)
                        background.GetComponent<SpriteRenderer>().sprite = sprites.topRight;
                    else if (h == 0)
                        background.GetComponent<SpriteRenderer>().sprite = sprites.bottomRight;
                    else
                        background.GetComponent<SpriteRenderer>().sprite = sprites.center;
                }
                else
                    background.GetComponent<SpriteRenderer>().sprite = sprites.center;
                background.GetComponent<SpriteRenderer>().color = colorChooserBackground;

                GameObject colorChooser = null;

                if (w == 0 && h == 1)
                {
                    colorChooser = Instantiate(iconPrefab);
                    colorChooser.GetComponent<SpriteRenderer>().sortingOrder = 1;
                }
                else
                {
                    if (colorCounter < availableColors.Count)
                    {
                        colorChooser = Instantiate(colorChooserPrefab);
                        colorChooser.GetComponent<ColorChooser>().color = availableColors[colorCounter];
                        colorCounter++;
                    }
                }

                background.transform.SetParent(colorBoardObject.transform);
                background.transform.localPosition = new Vector2(w, h);
                if (colorCounter - 1 < availableColors.Count && colorChooser != null)
                {
                    colorChooser.transform.SetParent(colorBoardObject.transform);
                    colorChooser.transform.localPosition = new Vector2(w, h);
                }
            }


            colorBoardObject.transform.SetParent(boardObject.transform);
            colorBoardObject.transform.localPosition = new Vector3(-(colorWidth + 1), map.height + 1, 0);


        }
        #endregion

        #region Horizontal Tips

        int horizontalMaxTips = 3;

        for (int i = 0; i < map.height; i++)
        {
            Color actualColor = backgroundColor;
            int tipCount = 0;
            int timesCount = 1;
            int localMax = 0;

            for (int j = (map.width - 1); j >= 0; j--)
            {
                if (board[j, i].Equals(backgroundColor) || !actualColor.Equals(board[j, i]))
                {
                    if (!actualColor.Equals(backgroundColor))
                    {
                        GameObject tipObject = Instantiate(tipSquarePrefab);
                        tipObject.transform.SetParent(boardObject.transform);
                        tipObject.transform.localPosition = new Vector2(-timesCount, i);
                        tipObject.GetComponent<SpriteRenderer>().color = actualColor;
                        float gamma = (float)(0.2126f * actualColor.r + 0.7152f * actualColor.g + 0.0722 * actualColor.b);
                        gamma = (gamma > 0.5f) ? 0 : 1;
                        TextMesh text = tipObject.GetComponentInChildren<TextMesh>();
                        if (text != null)
                        {
                            text.text = tipCount.ToString();
                            text.color = new Color(gamma, gamma, gamma);
                        }
                        timesCount++;
                        localMax++;
                        tipCount = 0;
                        actualColor = backgroundColor;
                    }
                }

                if (!board[j, i].Equals(backgroundColor))
                {
                    if (actualColor.Equals(backgroundColor))
                    {
                        actualColor = board[j, i];
                        tipCount++;
                    }
                    else if (actualColor.Equals(board[j, i]))
                        tipCount++;
                }

                if (j == 0)
                {
                    if (!actualColor.Equals(backgroundColor))
                    {
                        GameObject tipObject = Instantiate(tipSquarePrefab);
                        tipObject.transform.SetParent(boardObject.transform);
                        tipObject.transform.localPosition = new Vector2(-timesCount, i);
                        tipObject.GetComponent<SpriteRenderer>().color = actualColor;
                        float gamma = (float)(0.2126f * actualColor.r + 0.7152f * actualColor.g + 0.0722 * actualColor.b);
                        gamma = (gamma > 0.5f) ? 0 : 1;
                        TextMesh text = tipObject.GetComponentInChildren<TextMesh>();
                        if (text != null)
                        {
                            text.text = tipCount.ToString();
                            text.color = new Color(gamma, gamma, gamma);
                        }
                        timesCount++;
                        localMax++;
                        tipCount = 0;
                        actualColor = backgroundColor;
                    }
                }

            }

            if (localMax > horizontalMaxTips)
                horizontalMaxTips = localMax;

            if (timesCount == 1)
            {
                GameObject tipObject = Instantiate(tipSquarePrefab);
                tipObject.transform.SetParent(boardObject.transform);
                tipObject.transform.localPosition = new Vector2(-timesCount, i);
                tipObject.GetComponent<SpriteRenderer>().color = Color.white;
                TextMesh text = tipObject.GetComponentInChildren<TextMesh>();
                if (text != null)
                {
                    text.text = tipCount.ToString();
                    text.color = Color.black;
                }
            }

        }

        #endregion

        #region Vertical Tips

        int verticalMaxTips = 3;

        for (int j = 0; j < map.width; j++)
        {
            Color actualColor = backgroundColor;
            int tipCount = 0;
            int timesCount = 0;
            int localMax = 0;

            for (int i = 0; i < map.height; i++)
            {
                if (board[j, i].Equals(backgroundColor) || !actualColor.Equals(board[j, i]))
                {
                    if (!actualColor.Equals(backgroundColor))
                    {
                        GameObject tipObject = Instantiate(tipSquarePrefab);
                        tipObject.transform.SetParent(boardObject.transform);
                        tipObject.transform.localPosition = new Vector2(j, map.height + timesCount);
                        tipObject.GetComponent<SpriteRenderer>().color = actualColor;
                        float gamma = (float)(0.2126f * actualColor.r + 0.7152f * actualColor.g + 0.0722 * actualColor.b);
                        gamma = (gamma > 0.5f) ? 0 : 1;
                        TextMesh text = tipObject.GetComponentInChildren<TextMesh>();
                        if (text != null)
                        {
                            text.text = tipCount.ToString();
                            text.color = new Color(gamma, gamma, gamma);
                        }
                        timesCount++;
                        localMax++;
                        tipCount = 0;
                        actualColor = backgroundColor;
                    }
                }
                if (!board[j, i].Equals(backgroundColor))
                {
                    if (actualColor.Equals(backgroundColor))
                    {
                        actualColor = board[j, i];
                        tipCount++;
                    }
                    else if (actualColor.Equals(board[j, i]))
                        tipCount++;
                }
                if (i == (map.height - 1))
                {
                    if (!actualColor.Equals(backgroundColor))
                    {
                        GameObject tipObject = Instantiate(tipSquarePrefab);
                        tipObject.transform.SetParent(boardObject.transform);
                        tipObject.transform.localPosition = new Vector2(j, map.height + timesCount);
                        tipObject.GetComponent<SpriteRenderer>().color = actualColor;
                        float gamma = (float)(0.2126f * actualColor.r + 0.7152f * actualColor.g + 0.0722 * actualColor.b);
                        gamma = (gamma > 0.5f) ? 0 : 1;
                        TextMesh text = tipObject.GetComponentInChildren<TextMesh>();
                        if (text != null)
                        {
                            text.text = tipCount.ToString();
                            text.color = new Color(gamma, gamma, gamma);
                        }
                        timesCount++;
                        localMax++;
                        tipCount = 0;
                        actualColor = backgroundColor;
                    }
                }

            }

            if (localMax > verticalMaxTips)
                verticalMaxTips = localMax;

            if (timesCount == 0)
            {
                GameObject tipObject = Instantiate(tipSquarePrefab);
                tipObject.transform.SetParent(boardObject.transform);
                tipObject.transform.localPosition = new Vector2(j, map.height + timesCount);
                tipObject.GetComponent<SpriteRenderer>().color = Color.white;
                TextMesh text = tipObject.GetComponentInChildren<TextMesh>();
                if (text != null)
                {
                    text.text = tipCount.ToString();
                    text.color = Color.black;
                }
            }

        }

        #endregion

        #region Lines

        for (int i = 0; i < map.width; i++)
        {
            GameObject lineObject = Instantiate(linePrefab);
            lineObject.transform.SetParent(boardObject.transform);
            lineObject.transform.localPosition = new Vector2(-0.5f - (horizontalMaxTips - map.width) / 2, i);
            lineObject.transform.localScale = new Vector3(map.width + horizontalMaxTips, 1, 1);
            lineObject.GetComponent<Line>().colorVariant = ((i % 2) == 0) ? false : true;
            lineObject.GetComponent<Line>().LowLight();
            lineObject.GetComponent<Line>().coordinates = new Vector2(-1, i);

            if (i % 5 == 4 && i != map.width - 1)
            {
                GameObject drawLine = Instantiate(iconPrefab);
                drawLine.GetComponent<SpriteRenderer>().sprite = sprites.center;
                drawLine.GetComponent<SpriteRenderer>().color = new Color(0.1485f, 0.1953f, 0.21875f, 0.5f);
                drawLine.transform.SetParent(lineObject.transform);
                drawLine.transform.localScale = new Vector3(1, 0.1f, 1);
                drawLine.transform.localPosition = new Vector3(0f, 0.5f, 0f);
            }
        }
        
        for (int i = 0; i < map.height; i++)
        {
            GameObject lineObject = Instantiate(linePrefab);
            lineObject.transform.SetParent(boardObject.transform);
            lineObject.transform.localPosition = new Vector2(i, map.height - 0.5f + (verticalMaxTips - map.height) / 2);
            lineObject.transform.localScale = new Vector3(1, verticalMaxTips + map.height, 1);
            lineObject.GetComponent<Line>().colorVariant = ((i % 2) == 0) ? false : true;
            lineObject.GetComponent<Line>().LowLight();
            lineObject.GetComponent<Line>().coordinates = new Vector2(i, -1);

            if (i % 5 == 4 && i != map.height - 1)
            {
                GameObject drawLine = Instantiate(iconPrefab);
                drawLine.GetComponent<SpriteRenderer>().sprite = sprites.center;
                drawLine.GetComponent<SpriteRenderer>().color = new Color(0.1485f, 0.1953f, 0.21875f, 0.5f);
                drawLine.transform.SetParent(lineObject.transform);
                drawLine.transform.localScale = new Vector3(0.1f, 1f, 1f);
                drawLine.transform.localPosition = new Vector3(0.5f, 0f, 0f);
            }
        }

        #endregion

        level = -1;

        float scale = 0;
        float margin = 0;
        Vector3 screenDimensions = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, 0));

        margin = screenDimensions.y * 0.05f;
        scale = ((screenDimensions.y - margin * 2) * 2 / (verticalMaxTips + map.height));

        boardObject.transform.localScale = new Vector3(scale, scale, 1);
        boardObject.transform.position = new Vector3(screenDimensions.x - 0.5f - map.width * scale, -screenDimensions.y + margin + 0.5f, 0);
    }
}
