using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject rodPrefab;
    [SerializeField] private GameObject nutPrefab;
    [SerializeField] private float rodSpacing = 2.0f;
    [SerializeField] private Transform rodContainer;

    public Rod[] GenerateLevel(int rodCount, int nutColors, int nutsPerColor)
    {
        List<ColorType> allNuts = new List<ColorType>();
        for (int c = 0; c < nutColors; c++)
        {
            for (int i = 0; i < nutsPerColor; i++)
                allNuts.Add((ColorType)c);
        }
        Shuffle(allNuts);

        foreach (var rod in FindObjectsOfType<Rod>())
            Destroy(rod.gameObject);

        Rod[] rods = new Rod[rodCount];
        for (int i = 0; i < rodCount; i++)
        {
            Vector3 position = new Vector3(i * rodSpacing - ((rodCount - 1) * rodSpacing / 2f), 0f, 0f);
            GameObject rodObj = Instantiate(rodPrefab, position, Quaternion.identity);
            rodObj.transform.SetParent(rodContainer);///
            rods[i] = rodObj.GetComponent<Rod>();
        }

        int currentNut = 0;
        int totalNuts = nutColors * nutsPerColor;
        int totalCapacity = rodCount * rods[0].GetCapacity; // Thêm hàm GetCapacity() trong Rod nếu chưa có
        Debug.Log($"[LevelGenerator] Total nuts: {totalNuts}, Total rod capacity: {totalCapacity}");

        for (int i = 0; i < totalNuts; i++)
        {
            int tryCount = 0;
            Rod targetRod;
            do
            {
                targetRod = rods[Random.Range(0, rodCount)];
                tryCount++;
                if (tryCount > 100)
                {
                    Debug.LogError($"[LevelGenerator] Could not find empty rod for nut {i} after 100 tries!");
                    break;
                }
            } while (targetRod.IsFull());

            ColorType colorType = allNuts[currentNut++];
            Color color = GetColorFromType(colorType);
            GameObject nutObj = Instantiate(nutPrefab);
            Nut nut = nutObj.GetComponent<Nut>();
            nut.Initialize(colorType, color, targetRod);
            targetRod.PlaceNut(nut, true);

            Debug.Log($"[LevelGenerator] Placed nut {i} ({colorType}) in rod {System.Array.IndexOf(rods, targetRod)}. Rod now has {targetRod.GetNutCount} nuts.");
        }

        GameManager.Instance.RegisterRods(rods);
        return rods;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(i, list.Count);
            (list[i], list[rnd]) = (list[rnd], list[i]);
        }
    }

    private Color GetColorFromType(ColorType colorType)
    {
        switch (colorType)
        {
            case ColorType.Red:
                return Color.red;
            case ColorType.Green:
                return Color.green;
            case ColorType.Blue:
                return Color.blue;
            case ColorType.Yellow:
                return Color.yellow;
            case ColorType.Orange:
                return new Color(255, 126, 0, 1);
            case ColorType.Purple:
                return new Color(255, 0, 255, 1);
            default:
                return Color.gray;
        }
    }
}