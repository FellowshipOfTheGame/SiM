using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

[CreateAssetMenu(fileName = "Levels", menuName = "Levels/Create", order = 1)]
public class Collection : ScriptableObject
{
    public Texture2D[] levels;
    public Color[] backgrounds;
}
