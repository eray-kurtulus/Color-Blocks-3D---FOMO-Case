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
    }
}

public enum GameState
{
    Running,
    Win,
    Fail
}
