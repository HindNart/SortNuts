using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject rodPrefab;
    [SerializeField] private GameObject nutPrefab;
    [SerializeField] private float rodSpacing = 2.0f;
    [SerializeField] private Transform rodContainer;

    public Rod[] GenerateLevel(int rodCount, int nutColors, int nutsPerColor, float hiddenNutRatio = 0.2f)
    {
        // Tạo màu cho nut
        List<ColorType> allNuts = new List<ColorType>();
        for (int c = 0; c < nutColors; c++)
        {
            for (int i = 0; i < nutsPerColor; i++)
                allNuts.Add((ColorType)c);
        }
        Shuffle(allNuts);
        //

        foreach (var rod in FindObjectsOfType<Rod>())
            Destroy(rod.gameObject);

        // Tạo rod
        Rod[] rods = new Rod[rodCount];
        int rodsPerRow = 3;
        float rowSpacing = 3f;
        for (int i = 0; i < rodCount; i++)
        {
            int row = i / rodsPerRow;
            int col = i % rodsPerRow;
            float x = col * rodSpacing - ((Mathf.Min(rodCount, rodsPerRow) - 1) * rodSpacing / 2f);
            float z = -row * rowSpacing;
            Vector3 position = new Vector3(x, rodContainer.transform.localPosition.y, z);
            GameObject rodObj = Instantiate(rodPrefab, position, Quaternion.identity, rodContainer);
            rods[i] = rodObj.GetComponent<Rod>();
        }
        //

        // Tạo nut
        int currentNut = 0;
        int totalNuts = nutColors * nutsPerColor;
        int totalCapacity = rodCount * rods[0].GetCapacity;
        Debug.Log($"[LevelGenerator] Total nuts: {totalNuts}, Total rod capacity: {totalCapacity}");

        // Tạo danh sách index nut sẽ bị ẩn màu
        int hiddenNutCount = Mathf.RoundToInt(totalNuts * hiddenNutRatio);
        HashSet<int> hiddenNutIndexes = new HashSet<int>();
        while (hiddenNutIndexes.Count < hiddenNutCount)
            hiddenNutIndexes.Add(Random.Range(0, totalNuts));

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
            // Ẩn màu nếu index nằm trong danh sách
            bool hideColor = hiddenNutIndexes.Contains(i);
            //
            nut.Initialize(colorType, color, hideColor, targetRod);
            targetRod.PlaceNut(nut, true, true);

            Debug.Log($"[LevelGenerator] Placed nut {i} ({colorType}) in rod {System.Array.IndexOf(rods, targetRod)}. Rod now has {targetRod.GetNutCount} nuts.");
        }
        //

        // Đảm bảo nut trên cùng không bị ẩn màu
        foreach (var rod in rods)
        {
            Nut topNut = rod.PeekNut();
            if (topNut != null && topNut.IsColorHidden)
            {
                topNut.SetColorHidden(false);
            }
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

    public static Color GetColorFromType(ColorType colorType)
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
            case ColorType.White:
                return Color.white;
            case ColorType.Black:
                return Color.black;
            default:
                return Color.gray;
        }
    }
}