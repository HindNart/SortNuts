using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Rod : MonoBehaviour
{
    [SerializeField] private Transform nutContainer;
    [SerializeField] private ParticleSystem completeRodEff;
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
        if (ignoreColor)
            return nuts.Count < capacity;
        return nuts.Count < capacity && nuts.Peek().NutColor == nut.NutColor;
    }

    public void PlaceNut(Nut nut, bool ignoreColor = false, bool spawnLevel = false)
    {
        if (!CanPlaceNut(nut, ignoreColor)) return;
        nuts.Push(nut);
        nut.SetRod(this);
        nut.transform.SetParent(nutContainer);

        if (IsComplete())
        {
            completeRodEff.Play();
        }

        if (spawnLevel) // Nếu spawn level, cập nhật ngay lập tức
        {
            Vector3 pos = GetNutPosition(nuts.Count - 1);
            nut.transform.position = pos;
        }
        else
        {
            UpdateNutPositions(); // Di chuyển nhẹ khi không phải animation chính
        }
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

    public Vector3 GetNutPosition(int index)
    {
        // Vị trí nut theo index (0 là đáy)
        return transform.position + Vector3.up * (0.55f * index - 0.9f);
    }

    private void UpdateNutPositions()
    {
        Nut[] nutArray = nuts.ToArray();
        for (int i = 0; i < nutArray.Length; i++)
        {
            int targetIndex = nutArray.Length - 1 - i;
            Vector3 pos = GetNutPosition(targetIndex);
            nutArray[i].transform.DOMove(pos, 0.2f);
        }
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}