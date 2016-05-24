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
    [System.Serializable]
    public struct Sprites
    {
        public Sprite center;
        public Sprite topLeft;
        public Sprite topRight;
        public Sprite bottomLeft;
        public Sprite bottomRight;
    }
    public enum SpriteType
    {
        CENTER,
        TOP_LEFT,
        TOP_RIGHT,
        BOTTOM_LEFT,
        BOTTOM_RIGHT
    }
    public Sprites sprites;
    public Sprites xSprites;
    public GameObject pixelPrefab;

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

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit info;
            if(Physics.Raycast(ray, out info))
            {
                Pixel script = info.collider.gameObject.GetComponent<Pixel>();
                if (script)
                    script.OnSelect(currentColor);
            }
        }
    }

    void OnDestroy()
    {
        if(playerData != null)
            Save();
    }

    public static GameManager GetInstance()
    {
        //if (instance == null)
        //    instance = new GameManager();
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
                GameObject spriteObject = GameObject.Instantiate<GameObject>(pixelPrefab);
                spriteObject.transform.SetParent(boardObject.transform);
                spriteObject.transform.localPosition = new Vector2(j, i);
                Sprite sprite = sprites.center;
                spriteObject.GetComponent<Pixel>().type = SpriteType.CENTER;
                if(i == 0)
                {
                    if (j == 0)
                    {
                        sprite = sprites.bottomLeft;
                        spriteObject.GetComponent<Pixel>().type = SpriteType.BOTTOM_LEFT;
                    }
                    else if (j == map.width - 1)
                    {
                        sprite = sprites.bottomRight;
                        spriteObject.GetComponent<Pixel>().type = SpriteType.BOTTOM_RIGHT;
                    }
                }
                else if(i == map.height - 1)
                {
                    if (j == 0)
                    {
                        sprite = sprites.topLeft;
                        spriteObject.GetComponent<Pixel>().type = SpriteType.TOP_LEFT;
                    }
                    else if (j == map.width - 1)
                    {
                        sprite = sprites.topRight;
                        spriteObject.GetComponent<Pixel>().type = SpriteType.TOP_RIGHT;
                    }
                }
                spriteObject.GetComponent<SpriteRenderer>().sprite = sprite;
                //spriteObject.GetComponent<SpriteRenderer>().color = board[j, i];
            }
        }

    }
}
