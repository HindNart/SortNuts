using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI goldText;
    private int gold;
    private const string GOLD_KEY = "PLAYER_GOLD";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadGold();
        UpdateGoldUI();
    }

    public void AddGold(int amount)
    {
        gold += amount;
        SaveGold();
        UpdateGoldUI();
    }

    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            SaveGold();
            UpdateGoldUI();
            return true;
        }
        return false;
    }

    private void UpdateGoldUI()
    {
        if (goldText != null)
            goldText.text = $"Gold: {gold}";
    }

    private void SaveGold()
    {
        PlayerPrefs.SetInt(GOLD_KEY, gold);
        PlayerPrefs.Save();
    }

    private void LoadGold()
    {
        gold = PlayerPrefs.GetInt(GOLD_KEY, 0);
    }
}