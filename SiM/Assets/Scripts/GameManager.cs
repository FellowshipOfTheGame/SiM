using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;


    [Header("Level")]
    public Level levels;

    public Sprite sprite;
    public Sprite xSprite;

    public GameObject pixelPrefab;
    public GameObject hintPrefab;

    private Color[,] board;
    private GameObject boardObject;
    internal Color? currentColor = null;

    private class Hint
    {
        private GameObject unityObj;
        public Color color;
        public int size;

        public Hint(Color color, int size, GameObject unityObj)
        {
            this.color = color;
            this.size = System.Math.Min(size, 1);
            this.unityObj = unityObj;
        }
    }

    private List<LinkedList<Hint>> horizontal;
    private List<LinkedList<Hint>> vertical;

    public static GameManager GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        horizontal = new List<LinkedList<Hint>>();
        vertical = new List<LinkedList<Hint>>();
        LoadBoard(4);
    }

    void Update()
    {
    }

    void LoadBoard(int level)
    {
        Texture2D map = levels.maps[level];
        Color[] inlineBoard = map.GetPixels();
        board = new Color[map.width, map.height];
        if (boardObject != null)
            Destroy(boardObject);
        boardObject = new GameObject("Board");

        #region Instantiate objects (pixels)
        for (int i = 0; i < map.height; i++)
        {
            for (int j = 0; j < map.width; j++)
            {
                board[j, i] = inlineBoard[i * map.width + j];
                GameObject unityObj = GameObject.Instantiate<GameObject>(pixelPrefab);
                unityObj.transform.SetParent(boardObject.transform);
                unityObj.transform.localPosition = new Vector2(j, i);
                unityObj.GetComponent<Pixel>().OnSelect(board[j, i]);
            }
        }
        #endregion

        #region Create horizontal hints
        horizontal.Clear();
        for (int i = 0; i < map.height; i++)
        {
            horizontal.Add(new LinkedList<Hint>());
            Color current = Color.white;
            int size = 0;
            for (int j = 0; j < map.width; j++)
            {
                if (board[j, i].Equals(Color.white)) // Skip if white color
                {
                    if (!current.Equals(Color.white))
                        horizontal[i].AddLast(CreateHint(-(horizontal[i].Count + 1), i, current, size));
                    current = Color.white;
                    size = 0;
                }
                else if (!current.Equals(board[j, i])) // Change in color
                {
                    if (size > 0)
                        horizontal[i].AddLast(CreateHint(-(horizontal[i].Count + 1), i, current, size));
                    current = board[j, i];
                    size = 1;
                }
                else
                    size++;
            }
            if (!board[map.width - 1, i].Equals(Color.white))
                horizontal[i].AddLast(CreateHint(-(horizontal[i].Count + 1), i, current, size));
        }
        #endregion

        #region Create vertical hints
        vertical.Clear();
        for (int i = 0; i < map.width; i++)
        {
            vertical.Add(new LinkedList<Hint>());
            Color current = Color.white;
            int size = 0;
            for (int j = 0; j < map.height; j++)
            {
                if (board[i, j].Equals(Color.white)) // Skip if white color
                {
                    if (!current.Equals(Color.white))
                        vertical[i].AddLast(CreateHint(i, map.height + vertical[i].Count, current, size));
                    current = Color.white;
                    size = 0;
                }
                else if (!current.Equals(board[i, j])) // Change in color
                {
                    if(size > 0)
                        vertical[i].AddLast(CreateHint(i, map.height + vertical[i].Count, current, size));
                    current = board[i, j];
                    size = 1;
                }
                else
                    size++;
            }
            if (!board[i, map.height - 1].Equals(Color.white))
                vertical[i].AddLast(CreateHint(i, map.height + vertical[i].Count, current, size));
        }
        #endregion
    }

    private Hint CreateHint(int x, int y, Color color, int size)
    {
        GameObject unityObj = Instantiate<GameObject>(hintPrefab);
        unityObj.transform.SetParent(boardObject.transform);
        unityObj.transform.localPosition = new Vector2(x, y);
        unityObj.GetComponent<SpriteRenderer>().color = color;
        return new Hint(color, size, unityObj);
    }
}
