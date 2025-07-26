using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Rod[] rods;
    private CommandInvoker invoker;
    private IRuleValidator moveValidator;

    public int CurrentRodCount => rods.Length;

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
        invoker = new CommandInvoker();
        moveValidator = new BasicMoveValidator();
    }

    public void RegisterRods(Rod[] newRods)
    {
        rods = newRods;
    }

    public void RegisterNewRod(Rod newRod)
    {
        var rodList = new List<Rod>(rods) { newRod };
        rods = rodList.ToArray();
    }

    public void TryMove(Rod from, Rod to)
    {
        if (moveValidator.IsValidMove(from, to))
        {
            var move = new MoveCommand(from, to);
            invoker.ExecuteCommand(move);
            CheckWinCondition();
        }
    }

    public void UndoMove() => invoker.Undo();
    public void RedoMove() => invoker.Redo();

    private void CheckWinCondition()
    {
        foreach (Rod rod in rods)
        {
            if (rod.GetNutCount > 0 && !rod.IsComplete())
                return;
        }
        Debug.Log("Level Completed!");
        GoldManager.Instance.AddGold(50);
        LevelManager.Instance.NextLevel();
    }

    public void BackMainMenu()
    {
        if (Instance != null) Destroy(Instance.gameObject);
        if (GoldManager.Instance != null) Destroy(GoldManager.Instance.gameObject);
        if (LevelManager.Instance != null) Destroy(LevelManager.Instance.gameObject);
        if (AudioManager.Instance != null) Destroy(AudioManager.Instance.gameObject);
        SceneManager.LoadScene("MainMenu");
    }
}