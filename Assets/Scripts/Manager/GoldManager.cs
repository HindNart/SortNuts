using TMPro;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI goldTxt;
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
        if (goldTxt != null)
            goldTxt.text = $"{gold}";
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