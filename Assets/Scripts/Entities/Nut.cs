using UnityEngine;

public class Nut : MonoBehaviour
{
    public ColorType NutColor { get; private set; }
    public Rod CurrentRod { get; private set; }

    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void Initialize(ColorType colorType, Color color, Rod rod)
    {
        NutColor = colorType;
        SetColor(color);
        SetRod(rod);
    }

    public void SetColor(Color color)
    {
        if (_renderer == null)
            _renderer = GetComponent<Renderer>();
        if (_renderer != null)
            _renderer.material.color = color;
    }

    public void SetRod(Rod rod)
    {
        CurrentRod = rod;
    }
}