using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static float currentTime;
    [HideInInspector]
    public static bool hasWon;
    [HideInInspector]
    public static Color? currentColor;

    private static ColorChooser chosenColor;

    private bool isDragging;
    private int currentLevel;
    private Color? mouseColor;
    private BoardGenerator generator;

    void Start()
    {
        hasWon = false;
        currentTime = 0f;
        mouseColor = null;
        chosenColor = null;
        currentColor = null;
        currentLevel = BoardGenerator.level;
        generator = GetComponent<BoardGenerator>();
        generator.LoadBoard();
    }

    void Update()
    {
        if (hasWon)
            return;
        if(Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if(hit)
            {
                Pixel pixel = hit.collider.GetComponent<Pixel>();
                if(pixel)
                {
                    if (!isDragging)
                        mouseColor = pixel.color;
                    isDragging = true;
                    if(mouseColor.Equals(pixel.color))
                    {
                        if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
                            pixel.OnSelect(currentColor);
                        else if (Input.GetMouseButton(1) && !Input.GetMouseButton(0))
                            pixel.OnSelect(null);
                        if (CheckWin())
                            ScoreManager.AddScore(currentLevel, Mathf.RoundToInt(currentTime));
                    }
                }

                ColorChooser chooser = hit.collider.gameObject.GetComponent<ColorChooser>();
                if (chooser)
                {
                    if (chosenColor)
                        chosenColor.StartCoroutine("Shrink");
                    chooser.StartCoroutine("Grow");
                    currentColor = chooser.color;
                    chosenColor = chooser;
                }
            }
        }
        else if(!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            isDragging = false;
            currentColor = null;
        }
    }

    public bool CheckWin()
    {
        hasWon = true;

        GameObject[] pixels = GameObject.FindGameObjectsWithTag("Pixel");
        foreach (GameObject cell in pixels)
        {
            Pixel pixel = cell.GetComponent<Pixel>();

            if (pixel != null && !pixel.IsColorRight())
            {
                hasWon = false;
                break;
            }
        }
        if (hasWon)
        {
            foreach (GameObject cell in pixels)
            {
                Pixel pixel = cell.GetComponent<Pixel>();

                if (pixel)
                {
                    pixel.StopAllCoroutines();
                    pixel.StartCoroutine("Fill");
                }
            }
        }

        return hasWon;
    }
}
