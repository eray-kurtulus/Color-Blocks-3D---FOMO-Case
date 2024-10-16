using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public Camera mainCamera;   // Cache the camera for access
    public GameState currentGameState = GameState.Running;

    protected override void Awake()
    {
        base.Awake();

        mainCamera = Camera.main;
    }

    public void SetGameState(GameState state)
    {
        if (currentGameState == state) return;              // Don't set the same state twice
        if (currentGameState == GameState.Win) return;      // Don't change the state when already won
        if (currentGameState == GameState.Fail) return;     // Don't change the state when already failed

        switch (state)
        {
            case GameState.Running:
                break;
            case GameState.Win:
                UIManager.Instance.ShowWinUI();
                break;
            case GameState.Fail:
                UIManager.Instance.ShowFailUI();
                break;
        }

        currentGameState = state;
    }
}

public enum GameState
{
    Running,
    Win,
    Fail
}
