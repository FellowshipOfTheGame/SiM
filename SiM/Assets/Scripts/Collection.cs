using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

[CreateAssetMenu(fileName = "Levels", menuName = "Levels/Create", order = 1)]
public class Collection : ScriptableObject
{
    [System.Serializable]
    public class Level
    {
        public Texture2D texture;
        public Color background;
    }
    public Level[] levels;

    void OnEnable()
    {
        foreach (var level in levels)
        {
            if (level.background.Equals(new Color(0, 0, 0, 0)))
                level.background = Color.white;
            level.background.a = 1;
        }
    }
}
