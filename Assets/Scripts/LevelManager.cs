using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoSingleton<LevelManager>
{
    // This class loads and manages levels.
    public LevelData levelData;

    [SerializeField] private CellBehaviour cellPrefab = default;
    [SerializeField] private Block block1Prefab = default;
    [SerializeField] private Block block2Prefab = default;

    private int _currentLevel;
    private int _remainingMoves = -1;
    private CellBehaviour[,] _cellBehaviours;
    
    private void Awake()
    {
        base.Awake();

        _currentLevel = PlayerPrefs.GetInt("level", 1);
        
        LoadLevel(_currentLevel);
    }

    private void LoadLevel(int level)
    {
        // Read json file and create the level
        levelData = JsonUtility.FromJson<LevelData>(Resources.Load("Levels/Level" + _currentLevel).ToString());

        // Parse MoveLimit
        if (levelData.MoveLimit == 0)
        {
            UIManager.Instance.HideMovesText();
        }
        else
        {
            _remainingMoves = levelData.MoveLimit;
            UIManager.Instance.UpdateMovesText(_remainingMoves);
        }
        
        // Instantiate cells
        _cellBehaviours = new CellBehaviour[levelData.RowCount, levelData.ColCount];
        
        foreach (var cell in levelData.CellInfo)
        {
            CellBehaviour cb = Instantiate(cellPrefab);
            cb.SetCell(cell);
        }
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
