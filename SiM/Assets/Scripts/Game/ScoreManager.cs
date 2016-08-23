using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class ScoreManager : MonoBehaviour
{
    public TextAsset secretKey;

    private static string filename = "/player.dat";
    private static string addScoreURL = "http://www.fog.icmc.usp.br/sim/addScore.php";
    private static string getScoreURL = "http://www.fog.icmc.usp.br/sim/getScore.php";

    private static ScoreManager instance = null;
    private static PlayerData playerData = new PlayerData("Gustavo Ceccon", 8936822);

    [Serializable]
    private class PlayerData
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

        public void AddScore(int score)
        {
            if (scores == null)
                scores = new List<int>();
            scores.Add(score);
            isSync = false;
        }

        public int GetLevel()
        {
            if (scores != null)
                return scores.Count;
            return 0;
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
#if !UNITY_WEBGL
        if (Load())
            SceneManager.LoadScene("Menu");
#endif
    }

    public static void NewPlayer(string name, int id)
    {
        playerData = new PlayerData(name, id);
        Sync();
    }

    public static string GetName()
    {
        return playerData.name;
    }

    public static int GetLevel()
    {
        return playerData.GetLevel();
    }

    public static int GetScore(int level)
    {
        if (level < playerData.scores.Count)
            return playerData.scores[level];
        return -1;
    }

    public static void AddScore(int level, int score)
    {
		if (level < playerData.scores.Count) 
			return;
        playerData.AddScore(score);
    }

    public static bool Sync()
    {
        if (!playerData.isSync)
        {
#if UNITY_WEBGL
            instance.StartCoroutine(SyncCoroutine());
#else
            instance.Save();
#endif
        }
        return playerData.isSync;
    }

    private static bool CheckRequest(WWW request)
    {
        if (request == null)
        {
            Debug.LogError("Error with request");
            Debug.Log("Error with request");
            return false;
        }

        if (!string.IsNullOrEmpty(request.error))
        {
            Debug.LogError(request.error);
            Debug.Log("Error with request");
            return false;
        }

        if (request.text.StartsWith("Error"))
        {
            Debug.LogError(request.text);
            Debug.Log("Error with request");
            return false;
        }
        return true;
    }

    private static IEnumerator SyncCoroutine()
    {
        string secret = instance.secretKey.text.Replace("\n", "").Replace("\r", "");

        PlayerData localPlayer = playerData;
        PlayerData serverPlayer;
        WWWForm form = new WWWForm();
        WWW request;
        string hash = MD5.Md5Sum(localPlayer.name + localPlayer.id.ToString() + secret);

        form.AddField("hash", hash);
        form.AddField("name", localPlayer.name);
        form.AddField("id", localPlayer.id);
        request = new WWW(getScoreURL, form);

        yield return request;

        if (!CheckRequest(request))
            yield break;

        try
        {
            serverPlayer = JsonUtility.FromJson<PlayerData>(request.text);
        } catch(Exception e)
        {
            Debug.LogError(e.Message);
            yield break;
        }

        bool quit = false;
        if (serverPlayer.GetLevel() < localPlayer.GetLevel())
        {
            for (int level = serverPlayer.GetLevel(); !quit && level < localPlayer.GetLevel(); level++)
            {
                hash = MD5.Md5Sum(serverPlayer.name + serverPlayer.id + level + localPlayer.scores[level] + secret);

                form = new WWWForm();
                form.AddField("hash", hash);
                form.AddField("name", serverPlayer.name);
                form.AddField("id", serverPlayer.id);
                form.AddField("level", level);
                form.AddField("score", localPlayer.scores[level]);
                request = new WWW(addScoreURL, form);

                yield return request;

                if (!CheckRequest(request))
                    quit = true;

                if (!request.text.Equals("OK"))
                {
                    Debug.LogError(request.text);
                    quit = true;
                }

                if(!quit)
                    serverPlayer.AddScore(localPlayer.scores[level]);
            }
        }
        playerData = serverPlayer;
        playerData.isSync = !quit;
    }

    private bool Load()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        bool success = false;
        File.Delete(Application.persistentDataPath + filename);
        try
        {
            using (FileStream stream = File.Open(Application.persistentDataPath + filename, FileMode.Open))
                playerData = (PlayerData)formatter.Deserialize(stream);
            success = true;
            playerData.isSync = true;
        }
        catch (Exception)
        {
        }
        return success;
    }

    private bool Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        bool success = false;
        try
        {
            using (FileStream stream = File.Open(Application.persistentDataPath + filename, FileMode.Create))
                formatter.Serialize(stream, playerData);
            success = true;
            playerData.isSync = true;
        }
        catch (Exception)
        {
        }
        return success;
    }
}
