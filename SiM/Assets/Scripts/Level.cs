using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

[CreateAssetMenu(fileName = "Level", menuName = "Level/Create", order = 1)]
public class Level : ScriptableObject
{
    public Texture2D[] maps;
}
