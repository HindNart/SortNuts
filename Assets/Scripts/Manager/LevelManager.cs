using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private GameObject levelCompletedPanel;
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
        if (levelCompletedPanel != null)
        {
            levelCompletedPanel.SetActive(false);
        }

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
        levelGenerator.GenerateLevel(data.rodCount, data.nutColors, data.nutsPerColor, data.hiddenNutRatio);
    }

    public IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(1f);
        levelCompletedPanel.SetActive(true);
        levelCompletedPanel.transform.localScale = Vector3.zero;
        levelCompletedPanel.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);
        AudioManager.Instance.PlaySFX("LevelCompleted");
        yield return new WaitForSeconds(3f);
        levelCompletedPanel.SetActive(false);

        int next = currentLevelIndex + 1;
        if (next < levels.Length)
            StartLevel(next);
        else
            Debug.Log("All levels completed!");
    }

    public void RestartLevel()
    {
        AudioManager.Instance.PlaySFX("ButtonClick");

        if (GoldManager.Instance.SpendGold(10))
        {
            StartLevel(currentLevelIndex);
        }
        else Debug.Log("Not enough gold!");
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}