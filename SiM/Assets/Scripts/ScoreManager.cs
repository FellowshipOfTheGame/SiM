using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System;

public class ScoreManager : MonoBehaviour
{
    public TextAsset secretKeyAsset;

    private static ScoreManager instance = null;
    private static string secretKey = null;
    private static string playerDataFilename = "/player.dat";
    private static string addScoreURL = "http://www.fog.icmc.usp.br/sim/addScore.php";
    private static string getScoreURL = "http://www.fog.icmc.usp.br/sim/getScore.php";
    private PlayerData playerData = new PlayerData("Player", 0);

    private static bool waitingForServer = false;
    private static string Error
    {
        get
        {
            return _error;
        }
    }
    private static string _error = null;

    [System.Serializable]
    private struct PlayerData
    {
        public string name;
        public int id;
        public bool isSync;
        public List<int> scores;

        public PlayerData(string _name, int _id)
        {
            name = _name;
            id = _id;
            isSync = false;
            scores = new List<int>();
        }

        public int GetLevel()
        {
            return scores.Count;
        }

        public void AddScore(int time)
        {
            scores.Add(time);
            isSync = false;
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        if(Load())
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    void Update()
    {
    }

    public static ScoreManager GetInstance()
    {
        return instance;
    }

    public static void NewPlayer(string playerName, int id)
    {
        instance.playerData = new PlayerData(playerName, id);
        Sync();
    }

    public static int GetScore(int index)
    {
        if (instance.playerData.scores.Count < index)
            return instance.playerData.scores[index];
        return -1;
    }

    public static string GetPlayerName()
    {
        return instance.playerData.name;
    }

    public static int GetPlayerLevel()
    {
        return instance.playerData.GetLevel();
    }

    public static void AddScore(int score)
    {
        instance.playerData.AddScore(score);
    }

    public static bool IsSync()
    {
        return instance.playerData.isSync;
    }

    public static void Sync()
    {
        if (!IsSync() && !waitingForServer)
        {
            waitingForServer = true;
            instance.StartCoroutine(SyncCoroutine());
        }
    }

    private static IEnumerator SyncCoroutine()
    {
        if (secretKey == null)
            secretKey = instance.secretKeyAsset.text.Replace("\n", "").Replace("\r", "");

        PlayerData localPlayer = instance.playerData;
        WWWForm form = new WWWForm();
        string hash = MD5.Md5Sum(localPlayer.name + localPlayer.id.ToString() + secretKey);

        form.AddField("hash", hash);
        form.AddField("name", localPlayer.name);
        form.AddField("id", localPlayer.id.ToString());
        WWW webRequest = new WWW(getScoreURL, form);
        yield return webRequest;
        if (webRequest == null)
            yield break;

        if (string.IsNullOrEmpty(webRequest.error))
        {
            bool quit = false;
            if (webRequest.text.Contains("Error"))
            {
                _error = webRequest.text;
                yield break;
            }
            PlayerData serverPlayer = JsonUtility.FromJson<PlayerData>(webRequest.text);
            if (serverPlayer.GetLevel() < instance.playerData.GetLevel())
            {
                for (int i = serverPlayer.GetLevel(); !quit && i < localPlayer.GetLevel(); i++)
                {
                    hash = MD5.Md5Sum(serverPlayer.name + serverPlayer.id + i + localPlayer.scores[i] + secretKey);

                    form = new WWWForm();
                    form.AddField("hash", hash);
                    form.AddField("name", serverPlayer.name);
                    form.AddField("id", serverPlayer.id);
                    form.AddField("level", i);
                    form.AddField("score", localPlayer.scores[i]);
                    webRequest = new WWW(addScoreURL, form);
                    print(webRequest.url);
                    yield return webRequest;
                    if(webRequest == null)
                    {
                        _error = "Erro: Could not connect with server";
                        quit = true;
                    }
                    else if (string.IsNullOrEmpty(webRequest.error))
                    {
                        if(webRequest.text.Equals("OK"))
                            serverPlayer.AddScore(localPlayer.scores[i]);
                    }
                    else
                    {
                        _error = webRequest.error;
                        quit = true;
                    }

                }
            }
            if (!quit)
            {
                instance.playerData = serverPlayer;
                instance.Save();
            }
            else
                print("Server connection error");
        }
        else
        {
            print("Error getting score");
            if (webRequest.text.Contains("Error"))
            {
                _error = "Error getting score: " + webRequest.error;
                yield break;
            }
        }
        waitingForServer = true;
    }

    bool Load()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        bool success = false;
        File.Delete(Application.persistentDataPath + ScoreManager.playerDataFilename);
        try
        {
            using (FileStream stream = File.Open(Application.persistentDataPath + ScoreManager.playerDataFilename, FileMode.Open))
                playerData = (PlayerData)formatter.Deserialize(stream);
            success = true;
        }
        catch (Exception)
        {
        }
        return success;
    }

    bool Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        bool success = false;
        try
        {
            using (FileStream stream = File.Open(Application.persistentDataPath + ScoreManager.playerDataFilename, FileMode.Create))
                formatter.Serialize(stream, playerData);
            success = true;
        }
        catch (Exception)
        {
        }
        return success;
    }

}
