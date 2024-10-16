using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [ReadOnly] public Camera mainCamera;   // Cache the camera for access
    [ReadOnly] public GameState currentGameState = GameState.Running;
    [Space] 
    [SerializeField] private ParticleSystem confettiParticles = default;

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
                confettiParticles.Play(true);
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
