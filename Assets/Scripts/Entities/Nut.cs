using UnityEngine;

public class Nut : MonoBehaviour
{
    [SerializeField] private Material nutMat;
    [SerializeField] private Material hideColorNutMat;
    public ColorType NutColor { get; private set; }
    public Rod CurrentRod { get; private set; }
    private bool isColorHidden = false;
    public bool IsColorHidden => isColorHidden;

    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void Initialize(ColorType colorType, Color color, bool hideColor, Rod rod)
    {
        NutColor = colorType;
        SetColor(color);
        SetColorHidden(hideColor);
        SetRod(rod);
    }

    public void SetColor(Color color)
    {
        if (_renderer == null)
            _renderer = GetComponent<Renderer>();
        if (_renderer != null)
            _renderer.material.color = color;
    }

    public void SetColorHidden(bool hidden)
    {
        isColorHidden = hidden;
        if (_renderer == null)
            _renderer = GetComponent<Renderer>();
        if (_renderer != null)
        {
            if (hidden)
                _renderer.material = hideColorNutMat;
            else
            {
                _renderer.material = nutMat;
                _renderer.material.color = LevelGenerator.GetColorFromType(NutColor); // cần truyền đúng màu gốc
            }
        }
    }

    public void SetRod(Rod rod)
    {
        CurrentRod = rod;
    }
}