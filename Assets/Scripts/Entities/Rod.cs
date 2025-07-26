using System.Collections.Generic;
using UnityEngine;

public class Rod : MonoBehaviour
{
    [SerializeField] private Transform nutContainer;
    [SerializeField] private int capacity = 4;
    private readonly Stack<Nut> nuts = new Stack<Nut>();

    public int GetCapacity => capacity;
    public int GetNutCount => nuts.Count;

    public Nut PeekNut()
    {
        return nuts.Count > 0 ? nuts.Peek() : null;
    }

    public bool CanPlaceNut(Nut nut, bool ignoreColor)
    {
        if (nuts.Count == 0) return true;
        // return nuts.Count < capacity;
        if (ignoreColor)
            return nuts.Count < capacity;
        return nuts.Count < capacity && nuts.Peek().NutColor == nut.NutColor;
    }

    public void PlaceNut(Nut nut, bool ignoreColor = false)
    {
        if (!CanPlaceNut(nut, ignoreColor)) return;
        nuts.Push(nut);
        nut.SetRod(this);
        UpdateNutPositions();
        nut.transform.SetParent(nutContainer);///
    }

    // public void MoveNut(Nut nut)
    // {
    //     if (!CanPlaceNut(nut)) return;
    //     if (nuts.Peek().NutColor == nut.NutColor)
    //     {
    //         nuts.Push(nut);
    //         nut.SetRod(this);
    //         UpdateNutPositions();
    //         nut.transform.SetParent(nutContainer);///
    //     }
    // }

    public Nut RemoveNut()
    {
        return nuts.Count > 0 ? nuts.Pop() : null;
    }

    public bool IsFull()
    {
        return nuts.Count >= capacity;
    }

    public bool IsComplete()
    {
        if (nuts.Count == 0) return false;
        ColorType firstColor = nuts.Peek().NutColor;
        foreach (var nut in nuts)
        {
            if (nut.NutColor != firstColor)
                return false;
        }
        return nuts.Count == capacity;
    }

    private void UpdateNutPositions()
    {
        Nut[] nutArray = nuts.ToArray(); // Nut mới nhất ở index 0
        for (int i = 0; i < nutArray.Length; i++)
        {
            Vector3 pos = transform.position + Vector3.up * (0.55f * (nutArray.Length - 1 - i));
            nutArray[i].transform.position = pos - Vector3.up * 0.9f;
        }
    }
}