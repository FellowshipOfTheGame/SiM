using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance = null;
    private static string secretKey = "CkxO#12C2T@!ciN@$sLpL*X3cmUyK&_J";
    public static string playerDataFilename = "/player.dat";

    [HideInInspector]
    private GameData playerData = new GameData("Player", 0);


    [System.Serializable]
    public class ScoreException : System.Exception
    {
        public ScoreException() { }
        public ScoreException(string message) : base(message) { }
        public ScoreException(string message, System.Exception inner) : base(message, inner) { }
        protected ScoreException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }

    [System.Serializable]
    protected class GameData
    {
        public string playerName;
        public int playerId;
        public float[] scores;

        private GameData() { }

        public GameData(string playerName, int playerId)
        {
            this.playerName = playerName;
            this.playerId = playerId;
            this.scores = null;
        }

        public int GetLevel()
        {
            return (this.scores == null) ? 0 : this.scores.Length;
        }

        public void SetTime(int level, float time)
        {
            int currentLevel = 0;
            if (this.scores == null)
            {
                if (level != 0)
                    throw new ScoreException("Can't set level score, score level is greater from player level");
                this.scores = new float[level + 1];
            }
            else if (this.scores.Length != level)
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(this);
        Load();
    }

    public static ScoreManager GetInstance()
    {
        return instance;
    }

    public static string GetPlayerName()
    {
        return instance.playerData.playerName;
    }

    public static int GetPlayerLevel()
    {
        return instance.playerData.GetLevel();
    }

    public static void AddScore(int level, float score)
    {
        instance.playerData.SetTime(level, score);
    }

    void Load()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + ScoreManager.playerDataFilename))
        {
            FileStream stream = File.Open(Application.persistentDataPath + ScoreManager.playerDataFilename, FileMode.OpenOrCreate);
            try
            {
                playerData = (GameData)formatter.Deserialize(stream);
            }
            catch (SerializationException)
            {
                formatter.Serialize(stream, playerData);
            }
            stream.Close();
        }
    }

    void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Open(Application.persistentDataPath + ScoreManager.playerDataFilename, FileMode.Truncate);
        formatter.Serialize(stream, playerData);
        stream.Close();
    }
}
