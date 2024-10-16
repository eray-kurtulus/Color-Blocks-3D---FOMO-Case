using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoSingleton<LevelManager>
{
    // This class loads and manages levels.
    [ReadOnly] public LevelData levelData;
    
    [SerializeField] private int totalLevelCount = default;     // TODO this is not the best way to do this
    [SerializeField] private CellBehaviour cellPrefab = default;
    [SerializeField] private MovableBehaviour block1Prefab = default;
    [SerializeField] private MovableBehaviour block2Prefab = default;
    [SerializeField] private ExitBehaviour exitPrefab = default;

    private int _currentLevel;          // This will continue increasing
    private int _currentLevelIndex;     // This will return to 1 when all levels are played
    public int remainingMoves = -1;
    public int remainingMovables = -1;
    public CellBehaviour[,] cellBehaviours;
    private MovableBehaviour[] _blocks;
    private ExitBehaviour[] _exitBehaviours;
    
    protected override void Awake()
    {
        base.Awake();

        _currentLevel = PlayerPrefs.GetInt("level", 1);
        UIManager.Instance.UpdateLevelText(_currentLevel);

        _currentLevelIndex = (_currentLevel - 1) % totalLevelCount + 1; // LOOP the levels
        LoadLevel(_currentLevelIndex);
    }

    private void LoadLevel(int level)
    {
        // Read json file and create the level
        levelData = JsonUtility.FromJson<LevelData>(Resources.Load("Levels/Level" + _currentLevel).ToString());

        // Parse MoveLimit
        if (levelData.MoveLimit == 0)
        {
            // remainingMoves is -1 in this case, unlimited moves
            UIManager.Instance.HideMovesText();
        }
        else
        {
            remainingMoves = levelData.MoveLimit;
            UIManager.Instance.UpdateMovesText(remainingMoves);
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
        _blocks = new MovableBehaviour[levelData.MovableInfo.Length];
        Transform blocksParentTransform = new GameObject("Blocks Parent").transform;
        remainingMovables = levelData.MovableInfo.Length;

        for (var i = 0; i < levelData.MovableInfo.Length; i++)
        {
            Movable movable = levelData.MovableInfo[i];
            
            MovableBehaviour b;
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

    public void ExitedBlock()
    {
        remainingMovables--;

        if (remainingMovables == 0)
        {
            // No movables left, WIN
            GameManager.Instance.SetGameState(GameState.Win);
        }
    }
    
    public void MadeMove()
    {
        if (remainingMoves == -1) return;

        remainingMoves--;
        UIManager.Instance.UpdateMovesText(remainingMoves);
        if (remainingMoves == 0)
        {
            // No moves left, FAIL
            // NOTE that ExitedBlock will be called before this,
            // therefore if the player wins at the last move, it won't be a problem
            GameManager.Instance.SetGameState(GameState.Fail);
        }
    }
}
