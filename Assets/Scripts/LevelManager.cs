using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoSingleton<LevelManager>
{
    // This class loads and manages levels.
    
    private int _remainingMoves;
    private int _currentLevel;
    
    private void Awake()
    {
        _currentLevel = PlayerPrefs.GetInt("level", 1);
        
        LoadLevel(_currentLevel);
    }

    private void LoadLevel(int level)
    {
        // Read json file and create the level
    }

    public void LoadNextLevel()
    {
        PlayerPrefs.SetInt("level", _currentLevel + 1);
        
        ReloadScene();
    }
    
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
