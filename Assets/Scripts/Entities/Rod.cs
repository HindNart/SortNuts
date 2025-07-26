using System.Collections.Generic;
using UnityEngine;

public class Rod : MonoBehaviour
{
    [SerializeField] private Transform nutContainer;
    [SerializeField] private int capacity = 4;
    private readonly Stack<Nut> nuts = new Stack<Nut>();

    public bool CanPlaceNut(Nut nut)
    {
        if (nuts.Count == 0) return true;
        return nuts.Count < capacity && nuts.Peek().NutColor == nut.NutColor;
    }

    public void PlaceNut(Nut nut)
    {
        if (!CanPlaceNut(nut)) return;
        nut.transform.SetParent(nutContainer);///
        nuts.Push(nut);
        nut.SetRod(this);
        UpdateNutPositions();
    }

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
        int index = 0;
        foreach (Nut nut in nuts)
        {
            Vector3 pos = transform.position + Vector3.up * (0.5f * index);
            nut.transform.position = pos;
            index++;
        }
    }
}