using System.Collections.Generic;
using UnityEngine;

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
            if (!rod.IsComplete())
                return;
        }
        Debug.Log("Level Completed!");
        UIManager.Instance.AddGold(50);
        LevelManager.Instance.NextLevel();
    }
}