using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "NutSortPuzzle/Level Data")]
public class LevelData : ScriptableObject
{
    public int rodCount = 4;
    public int nutColors = 3;
    public int nutsPerColor = 4;
    public float hiddenNutRatio = 0.2f;
}
