using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    private static string secretKey = "CkxO#12C2T@!ciN@$sLpL*X3cmUyK&_J";

    public static string playerDataFilename = "/player.dat";
    private GameData playerData = null;

    [Header("Level")]
    public Level levels;
    [Header("Sprites")]
    public Sprite center;
    public Sprite topLeft;
    public Sprite topRight;
    public Sprite bottomLeft;
    public Sprite bottomRight;

    private Color[,] board;
    private GameObject boardObject;
    private int currentLevel = 0;
    internal Color? currentColor = null;

    [System.Serializable]
    public class GameData
    {
        public string playerName;
        public int playerId;
        public float[] time;

        public GameData()
        {
            playerName = "Player Name";
            playerId = 0;
            time = null;
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(this);
        Load();
    }

    void OnDestroy()
    {
        if(playerData != null)
            Save();
    }

    public static GameManager GetInstance()
    {
        if (instance == null)
            instance = new GameManager();
        return instance;
    }

    void Load()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + GameManager.playerDataFilename))
        {
            FileStream stream = File.Open(Application.persistentDataPath + GameManager.playerDataFilename, FileMode.Open);
            try
            {
                playerData = (GameData)formatter.Deserialize(stream);
            }catch(SerializationException)
            {
                playerData = new GameData();
                formatter.Serialize(stream, playerData);
            }
            stream.Close();
        }
        else
            playerData = new GameData();

        LoadBoard();
    }

    void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Open(Application.persistentDataPath + GameManager.playerDataFilename, FileMode.OpenOrCreate);
        formatter.Serialize(stream, playerData);
        stream.Close();
    }

    void LoadBoard()
    {
        if (playerData.time == null)
            currentLevel = 0;
        else
            currentLevel = Mathf.Min(levels.maps.Length, playerData.time.Length);
        Texture2D map = levels.maps[currentLevel];
        Color[] inlineBoard = map.GetPixels();
        board = new Color[map.width, map.height];
        if (boardObject != null)
            Destroy(boardObject);
        boardObject = new GameObject("Board");


        for (int i = 0; i < map.height; i++)
        {
            for (int j = 0; j < map.width; j++)
            {
                board[j, i] = inlineBoard[i * map.width + j];
                GameObject spriteObject = new GameObject("Pixel" + j + "," + i);
                spriteObject.transform.SetParent(boardObject.transform);
                spriteObject.transform.localPosition = new Vector2(j, i);
                spriteObject.AddComponent<SpriteRenderer>();
                Sprite sprite = center;
                if(i == 0)
                {
                    if (j == 0)
                        sprite = bottomLeft;
                    else if (j == map.width - 1)
                        sprite = bottomRight;
                }
                else if(i == map.height - 1)
                {
                    if (j == 0)
                        sprite = topLeft;
                    else if (j == map.width - 1)
                        sprite = topRight;
                }
                spriteObject.GetComponent<SpriteRenderer>().sprite = sprite;
                spriteObject.GetComponent<SpriteRenderer>().color = board[j, i];
            }
        }

    }
}
