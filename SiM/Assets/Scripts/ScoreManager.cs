using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance = null;
    private GameData playerData = null;

    private string secretKey;
    public TextAsset secretKeyFile;

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

        public int GetCurrentLevel()
        {
            return (this.scores == null) ? 0 : this.scores.Length;
        }

        public void SetTime(int level, float time)
        {
            if (this.scores == null)
                this.scores = new float[level + 1];
            if(this.scores.Length < level)
            {
                float[] aux = new float[level + 1];
                System.Array.Copy(this.scores, aux, this.scores.Length);
                this.scores = aux;
            }
            this.scores[level] = time;
        }
    }

    #region Static functions
    public static ScoreManager GetInstance()
    {
        return instance;
    }

    public static string GetPlayerName()
    {
        return instance.playerData.playerName;
    }

    public static int GetPlayerCurrentLevel()
    {
        return instance.playerData.GetCurrentLevel();
    }

    public static void AddScore(int level, float score)
    {
        instance.playerData.SetTime(level, score);
    }

    #endregion

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
        if(secretKeyFile != null)
            secretKey = secretKeyFile.text;
        Load();
    }

    void Load()
    {
        if (playerData != null)
            return;
    }

    void Save()
    {
        if (playerData == null)
            return;
    }

    void OnDestroy()
    {
        if (secretKeyFile != null)
            secretKey = secretKeyFile.text;
        Save();
    }
}
