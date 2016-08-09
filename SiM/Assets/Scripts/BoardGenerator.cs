using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardGenerator : MonoBehaviour
{

    [Header("Levels")]
    public Collection levels;
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

    public GameObject pixelPrefab;
    public GameObject linePrefab;
    public GameObject colorChooserPrefab;
    public GameObject tipSquarePrefab;
    public GameObject iconPrefab;

    private Color[,] board;
    private GameObject boardObject;
    private GameObject colorBoardObject;


    public void LoadBoard()
    {
        if (level == -1)
            return;
        Texture2D map = levels.levels[level];
        Color[] inlineBoard = map.GetPixels();
        List<Color?> availableColors = new List<Color?>();
        Color backgroundColor = levels.backgrounds[level];
        board = new Color[map.width, map.height];
        if (boardObject != null)
            Destroy(boardObject);
        boardObject = new GameObject("Board");
        availableColors.Add(backgroundColor);
        availableColors.Add(null);


        GameObject bObject = GameObject.Instantiate<GameObject>(linePrefab);
        bObject.transform.SetParent(boardObject.transform);
        bObject.transform.localPosition = new Vector2(map.width / 2, map.height / 2);
        bObject.transform.localScale = new Vector3(map.width, map.height, 1);
        bObject.GetComponent<Line>().coordinates = new Vector2(-1, -1);

        for (int i = 0; i < map.height; i++)
        {
            for (int j = 0; j < map.width; j++)
            {
                board[j, i] = inlineBoard[i * map.width + j];
                GameObject spriteObject = GameObject.Instantiate<GameObject>(pixelPrefab);
                spriteObject.transform.SetParent(boardObject.transform);
                spriteObject.transform.localPosition = new Vector2(j, i);

                spriteObject.GetComponent<Pixel>().InitializeColors(board[j, i], backgroundColor);
                spriteObject.GetComponent<Pixel>().coordinates = new Vector2(j, i);

                if (!availableColors.Contains(board[j, i]))
                    availableColors.Add(board[j, i]);
            }

        }


        //Create colors choser
        if (colorBoardObject != null)
            Destroy(colorBoardObject);
        colorBoardObject = new GameObject("Color Picker");

        int colorWidth = (availableColors.Count - 2 > 3) ? 4 : 3;
        int colorCounter = 0;

        GameManager.currentColor = availableColors[2];

        for (int h = 1; h >= 0; h--)
        {
            for (int w = 0; w < colorWidth; w++)
            {

                GameObject backSquare = GameObject.Instantiate<GameObject>(iconPrefab);
                if (w == 0)
                {
                    if (h == 1)
                        backSquare.GetComponent<SpriteRenderer>().sprite = sprites.topLeft;
                    else if (h == 0)
                        backSquare.GetComponent<SpriteRenderer>().sprite = sprites.bottomLeft;
                    else
                        backSquare.GetComponent<SpriteRenderer>().sprite = sprites.center;
                }
                else if (w == (colorWidth - 1))
                {
                    if (h == 1)
                        backSquare.GetComponent<SpriteRenderer>().sprite = sprites.topRight;
                    else if (h == 0)
                        backSquare.GetComponent<SpriteRenderer>().sprite = sprites.bottomRight;
                    else
                        backSquare.GetComponent<SpriteRenderer>().sprite = sprites.center;
                }
                else
                    backSquare.GetComponent<SpriteRenderer>().sprite = sprites.center;
                backSquare.GetComponent<SpriteRenderer>().color = new Color(0.7382f, 0.7382f, 0.7382f);

                GameObject colorObject = null;

                if (w == 0 && h == 1)
                {
                    colorObject = GameObject.Instantiate<GameObject>(iconPrefab);
                    colorObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
                }
                else
                {
                    if (colorCounter < availableColors.Count)
                    {
                        colorObject = GameObject.Instantiate<GameObject>(colorChooserPrefab);
                        colorObject.GetComponent<ColorChooser>().setColor(availableColors[colorCounter]);
                        colorCounter++;
                    }
                }

                backSquare.transform.SetParent(colorBoardObject.transform);
                backSquare.transform.localPosition = new Vector2(w, h);
                if (colorCounter - 1 < availableColors.Count && colorObject != null)
                {
                    colorObject.transform.SetParent(colorBoardObject.transform);
                    colorObject.transform.localPosition = new Vector2(w, h);
                }

            }
        }

        colorBoardObject.transform.SetParent(boardObject.transform);
        colorBoardObject.transform.localPosition = new Vector3(-(colorWidth + 1), map.height + 1, 0);

        //Create tips 
        int horizontalMaxTips = 3;

        //Horizontal tips
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
                        GameObject tipObject = GameObject.Instantiate<GameObject>(tipSquarePrefab);
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
                        GameObject tipObject = GameObject.Instantiate<GameObject>(tipSquarePrefab);
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
                GameObject tipObject = GameObject.Instantiate<GameObject>(tipSquarePrefab);
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

        //Create Vertial Tips
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
                        GameObject tipObject = GameObject.Instantiate<GameObject>(tipSquarePrefab);
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
                        GameObject tipObject = GameObject.Instantiate<GameObject>(tipSquarePrefab);
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
                GameObject tipObject = GameObject.Instantiate<GameObject>(tipSquarePrefab);
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

        //Create Lines and Columns
        //Create Line
        for (int i = 0; i < map.width; i++)
        {
            GameObject lineObject = GameObject.Instantiate<GameObject>(linePrefab);
            lineObject.transform.SetParent(boardObject.transform);
            lineObject.transform.localPosition = new Vector2(-0.5f - (horizontalMaxTips - map.width) / 2, i);
            lineObject.transform.localScale = new Vector3(map.width + horizontalMaxTips, 1, 1);
            lineObject.GetComponent<Line>().colorVariant = ((i % 2) == 0) ? false : true;
            lineObject.GetComponent<Line>().lowlight();
            lineObject.GetComponent<Line>().coordinates = new Vector2(-1, i);

            if (i % 5 == 4 && i != map.width - 1)
            {
                GameObject drawLine = GameObject.Instantiate<GameObject>(iconPrefab);
                drawLine.GetComponent<SpriteRenderer>().sprite = sprites.center;
                drawLine.GetComponent<SpriteRenderer>().color = new Color(0.1485f, 0.1953f, 0.21875f, 0.5f);
                drawLine.transform.SetParent(lineObject.transform);
                drawLine.transform.localScale = new Vector3(1, 0.1f, 1);
                drawLine.transform.localPosition = new Vector3(0f, 0.5f, 0f);
            }
        }

        //Create Columns
        for (int i = 0; i < map.height; i++)
        {
            //Create Column
            GameObject lineObject = GameObject.Instantiate<GameObject>(linePrefab);
            lineObject.transform.SetParent(boardObject.transform);
            lineObject.transform.localPosition = new Vector2(i, map.height - 0.5f + (verticalMaxTips - map.height) / 2);
            lineObject.transform.localScale = new Vector3(1, verticalMaxTips + map.height, 1);
            lineObject.GetComponent<Line>().colorVariant = ((i % 2) == 0) ? false : true;
            lineObject.GetComponent<Line>().lowlight();
            lineObject.GetComponent<Line>().coordinates = new Vector2(i, -1);

            if (i % 5 == 4 && i != map.height - 1)
            {
                GameObject drawLine = GameObject.Instantiate<GameObject>(iconPrefab);
                drawLine.GetComponent<SpriteRenderer>().sprite = sprites.center;
                drawLine.GetComponent<SpriteRenderer>().color = new Color(0.1485f, 0.1953f, 0.21875f, 0.5f);
                drawLine.transform.SetParent(lineObject.transform);
                drawLine.transform.localScale = new Vector3(0.1f, 1f, 1f);
                drawLine.transform.localPosition = new Vector3(0.5f, 0f, 0f);
            }
        }

        Vector3 screenDimensions = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, 0));
        float scale = 0;
        float margin = 0;
        margin = screenDimensions.y * 0.05f;
        scale = ((screenDimensions.y - margin * 2) * 2 / (verticalMaxTips + map.height));
        boardObject.transform.localScale = new Vector3(scale, scale, 1);
        boardObject.transform.position = new Vector3(screenDimensions.x - 0.5f - map.width * scale, -screenDimensions.y + margin + 0.5f, 0);
        level = -1;
    }
}
