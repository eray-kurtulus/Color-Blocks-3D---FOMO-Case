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
    [SerializeField] private ExitBehaviour exitPrefab = default;

    private int _currentLevel;
    private int _remainingMoves = -1;
    public CellBehaviour[,] cellBehaviours;
    private Block[] _blocks;
    private ExitBehaviour[] _exitBehaviours;
    
    protected override void Awake()
    {
        base.Awake();

        _currentLevel = PlayerPrefs.GetInt("level", 4); // Default is 4 for testing purposes TODO
        
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
        
        // Instantiate Cells
        cellBehaviours = new CellBehaviour[levelData.RowCount, levelData.ColCount];
        Transform cellBehavioursParentTransform = new GameObject("Cell Behaviours Parent").transform;
        
        foreach (var cell in levelData.CellInfo)
        {
            CellBehaviour cb = Instantiate(cellPrefab, cellBehavioursParentTransform);
            cb.SetCell(cell);
            cellBehaviours[cell.Row, cell.Col] = cb;
        }
        
        // Instantiate Movable Blocks
        _blocks = new Block[levelData.MovableInfo.Length];
        Transform blocksParentTransform = new GameObject("Blocks Parent").transform;

        for (var i = 0; i < levelData.MovableInfo.Length; i++)
        {
            Movable movable = levelData.MovableInfo[i];
            
            Block b;
            if (movable.Length == 1)    // TODO Add more prefabs for longer blocks
            {
                b = Instantiate(block1Prefab, blocksParentTransform);
            }
            else
            {
                b = Instantiate(block2Prefab, blocksParentTransform);
            }
            b.SetMovable(movable);
            
            _blocks[i] = b;
        }

        // Instantiate Exits
        _exitBehaviours = new ExitBehaviour[levelData.ExitInfo.Length];
        Transform exitsParentTransform = new GameObject("Exits Parent").transform;

        for (var i = 0; i < levelData.ExitInfo.Length; i++)
        {
            Exit ei = levelData.ExitInfo[i];
            
            ExitBehaviour exitBehaviour = Instantiate(exitPrefab, exitsParentTransform);
            exitBehaviour.SetExitData(ei);
            
            _exitBehaviours[i] = exitBehaviour;
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
