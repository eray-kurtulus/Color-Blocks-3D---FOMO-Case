using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameState CurrentGameState = GameState.Running;
    
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
