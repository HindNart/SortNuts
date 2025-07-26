using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private LevelData[] levels;
    [SerializeField] private LevelGenerator levelGenerator;
    private int currentLevelIndex;

    public int CurrentLevel => currentLevelIndex + 1;

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
        currentLevelIndex = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);
        StartLevel(currentLevelIndex);
    }

    public void StartLevel(int index)
    {
        if (index < 0 || index >= levels.Length)
        {
            Debug.Log("No more levels!");
            return;
        }

        if (levelTxt != null)
            levelTxt.text = $"LEVEL {index + 1}";

        currentLevelIndex = index;
        PlayerPrefs.SetInt("CURRENT_LEVEL", currentLevelIndex);

        var data = levels[currentLevelIndex];
        Debug.Log($"Level {CurrentLevel}: {data.rodCount} rods, {data.nutColors} colors");
        levelGenerator.GenerateLevel(data.rodCount, data.nutColors, data.nutsPerColor);
    }

    public void NextLevel()
    {
        int next = currentLevelIndex + 1;
        if (next < levels.Length)
            StartLevel(next);
        else
            Debug.Log("All levels completed!");
    }

    public void RestartLevel()
    {
        StartLevel(currentLevelIndex);
    }
}