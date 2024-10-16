using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MovableBehaviour : MonoBehaviour
{
    [SerializeField] private Texture[] colorUpDownTextures = default;
    [SerializeField] private Texture[] colorRightLeftTextures = default;
    [Space]
    [SerializeField] private LayerMask draggableLayer = default;
    [SerializeField] private float dragAmount = 0.2f;   // The amount of drag required to move a Movable
    [Space]
    
    private Movable _movable;
    private bool _isDragging;
    private Vector3 _mouseDownPos;
    private Vector3 _mouseCurrentPos;

    public void SetMovable(Movable movable)
    {
        this._movable = movable;
        
        // Positioning
        transform.Translate(new Vector3((movable.Col - (LevelManager.Instance.levelData.ColCount / 2f) + 0.5f) * 1f,
                                                0f,
                                                -(movable.Row - (LevelManager.Instance.levelData.RowCount / 2f) + 0.5f) * 1f));

        // Get material
        Material mat = transform.GetChild(0).GetComponent<MeshRenderer>().material;

        if (movable.Direction[0] == 0 || movable.Direction[1] == 0)
        {
            // Up-Down Direction
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            
            // Set material texture according to Color
            mat.mainTexture = colorUpDownTextures[movable.Colors];
        }
        else
        {
            // Right-Left Direction
            
            // Set material texture according to Color
            mat.mainTexture = colorRightLeftTextures[movable.Colors];
        }
        
        // Register this MovableBehaviour to the cell(s) it occupies
        UpdateOccupiedCells();
    }

    private void UpdateOccupiedCells(bool remove = false)
    {
        MovableBehaviour changeToMovableBehaviour = this;
        if (remove) changeToMovableBehaviour = null;
        
        LevelManager.Instance.cellBehaviours[_movable.Row, _movable.Col].SetOccupyingBlock(changeToMovableBehaviour);
        if (_movable.Length > 1)
        {
            // If the length is > 1, register this movableBehaviour to all the other cells it occupies
            for (int i = 1; i < _movable.Length; i++)
            {
                if (_movable.Direction[0] == 0 || _movable.Direction[1] == 0)
                {
                    // Go down
                    LevelManager.Instance.cellBehaviours[_movable.Row + i, _movable.Col].SetOccupyingBlock(changeToMovableBehaviour);
                }
                else
                {
                    // Go right
                    LevelManager.Instance.cellBehaviours[_movable.Row, _movable.Col + i].SetOccupyingBlock(changeToMovableBehaviour);
                }
            }
        }
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.currentGameState != GameState.Running) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, draggableLayer))
        {
            _mouseDownPos = hitInfo.point;
            _isDragging = true;
            Debug.Log("isDragging True");
        }
        else
        {
            Debug.LogWarning("OnMouseDown raycast doesn't hit the draggable plane");
        }
    }

    private void OnMouseDrag()
    {
        if (!_isDragging) return;
        if (GameManager.Instance.currentGameState != GameState.Running) return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, draggableLayer))
        {
            _mouseCurrentPos = hitInfo.point;
            if (_movable.Direction[0] == 0 || _movable.Direction[1] == 0)
            {
                // Up-Down direction
                if (_mouseDownPos.z - _mouseCurrentPos.z > dragAmount)
                {
                    // Move DOWN (2)
                    Debug.Log("DOWN");
                    
                    // Check downward cells
                    int amountMoved = 0;
                    int row;
                    for (row = _movable.Row + _movable.Length; row < LevelManager.Instance.levelData.RowCount; row++)
                    {
                        if (LevelManager.Instance.cellBehaviours[row, _movable.Col].IsOccupied())
                        {
                            // Occupied cell
                            Debug.Log("Cell r:" + row + " c:" + _movable.Col + " is occupied, amountMoved:" + amountMoved);
                            break;
                        }
                        else
                        {
                            // Empty cell
                            amountMoved++;
                        }
                    }

                    // Check if at the exit
                    if (row == LevelManager.Instance.levelData.RowCount)
                    {
                        // At the exit
                        // Check if the movableBehaviour can exit, and move
                        bool canExit = false;
                        foreach (var eb in LevelManager.Instance.cellBehaviours[row - 1, _movable.Col].attachedExitBehaviours)
                        {
                            if (eb.exitData.Direction == 2 && eb.exitData.Colors == _movable.Colors)
                            {
                                canExit = true;
                            }
                        }
                        
                        MoveInDirection(amountMoved, 2, canExit);
                    }
                    else
                    {
                        // Not at the exit
                        MoveInDirection(amountMoved, 2, false);
                    }
                    
                    _isDragging = false;
                }
                else if (_mouseCurrentPos.z - _mouseDownPos.z > dragAmount)
                {
                    // Move UP (0)
                    Debug.Log("UP");
                    
                    // Check upward cells
                    int amountMoved = 0;
                    int row;
                    for (row = _movable.Row - 1; row >= 0; row--)
                    {
                        if (LevelManager.Instance.cellBehaviours[row, _movable.Col].IsOccupied())
                        {
                            // Occupied cell
                            Debug.Log("Cell r:" + row + " c:" + _movable.Col + " is occupied, amountMoved:" + amountMoved);
                            break;
                        }
                        else
                        {
                            // Empty cell
                            amountMoved++;
                        }
                    }

                    // Check if at the exit
                    if (row == -1)
                    {
                        // At the exit
                        // Check if the movableBehaviour can exit, and move
                        bool canExit = false;
                        foreach (var eb in LevelManager.Instance.cellBehaviours[0, _movable.Col].attachedExitBehaviours)
                        {
                            if (eb.exitData.Direction == 0 && eb.exitData.Colors == _movable.Colors)
                            {
                                canExit = true;
                            }
                        }
                        
                        MoveInDirection(amountMoved, 0, canExit);
                    }
                    else
                    {
                        // Not at the exit
                        MoveInDirection(amountMoved, 0, false);
                    }

                    _isDragging = false;
                }
            }
            else
            {
                // Right-Left direction
                if (_mouseDownPos.x - _mouseCurrentPos.x > dragAmount)
                {
                    // Move LEFT (3)
                    Debug.Log("LEFT");
                    
                    // Check leftward cells
                    int amountMoved = 0;
                    int col;
                    for (col = _movable.Col -1; col >= 0; col--)
                    {
                        if (LevelManager.Instance.cellBehaviours[_movable.Row, col].IsOccupied())
                        {
                            // Occupied cell
                            Debug.Log("Cell r:" + _movable.Row + " c:" + col + " is occupied, amountMoved:" + amountMoved);
                            break;
                        }
                        else
                        {
                            // Empty cell
                            amountMoved++;
                        }
                    }

                    // Check if at the exit
                    if (col == -1)
                    {
                        // At the exit
                        // Check if the movableBehaviour can exit, and move
                        bool canExit = false;
                        foreach (var eb in LevelManager.Instance.cellBehaviours[_movable.Row, 0].attachedExitBehaviours)
                        {
                            if (eb.exitData.Direction == 3 && eb.exitData.Colors == _movable.Colors)
                            {
                                canExit = true;
                            }
                        }
                        
                        MoveInDirection(amountMoved, 3, canExit);
                    }
                    else
                    {
                        // Not at the exit
                        MoveInDirection(amountMoved, 3, false);
                    }
                    
                    _isDragging = false;
                }
                else if (_mouseCurrentPos.x - _mouseDownPos.x > dragAmount)
                {
                    // Move RIGHT (1)
                    Debug.Log("RIGHT");

                    // Check rightward cells
                    int amountMoved = 0;
                    int col;
                    for (col = _movable.Col + _movable.Length; col < LevelManager.Instance.levelData.ColCount; col++)
                    {
                        if (LevelManager.Instance.cellBehaviours[_movable.Row, col].IsOccupied())
                        {
                            // Occupied cell
                            Debug.Log("Cell r:" + _movable.Row + " c:" + col + " is occupied, amountMoved:" + amountMoved);
                            break;
                        }
                        else
                        {
                            // Empty cell
                            amountMoved++;
                        }
                    }

                    // Check if at the exit
                    if (col == LevelManager.Instance.levelData.ColCount)
                    {
                        // At the exit
                        // Check if the movableBehaviour can exit, and move
                        bool canExit = false;
                        foreach (var eb in LevelManager.Instance.cellBehaviours[_movable.Row, col - 1].attachedExitBehaviours)
                        {
                            if (eb.exitData.Direction == 1 && eb.exitData.Colors == _movable.Colors)
                            {
                                canExit = true;
                            }
                        }
                        
                        MoveInDirection(amountMoved, 1, canExit);
                    }
                    else
                    {
                        // Not at the exit
                        MoveInDirection(amountMoved, 1, false);
                    }
                    
                    _isDragging = false;
                }
            }
        }
        else
        {
            Debug.LogWarning("OnMouseDrag raycast doesn't hit the draggable plane");
        }
    }

    private void OnMouseUp()
    {
        _isDragging = false;
    }

    private void MoveInDirection(int amountMoved, int direction, bool hasExited)
    {
        // Directions:
        //     0: Up
        //     1: Right
        //     2: Down
        //     3: Left

        if (amountMoved == 0) return;
        
        // Clear the occupied cells
        UpdateOccupiedCells(true);

        if (hasExited)
        {
            // With exit
            LevelManager.Instance.ExitedBlock();

            switch (direction)
            {
                case 0:
                    // Move amountMoved amount UP
                    TweenExitMovement(amountMoved, Vector3.forward);
                    break;
                case 1:
                    // Move amountMoved amount RIGHT
                    TweenExitMovement(amountMoved, Vector3.right);
                    break;
                case 2:
                    // Move amountMoved amount DOWN
                    TweenExitMovement(amountMoved, Vector3.back);
                    break;
                case 3:
                    // Move amountMoved amount LEFT
                    TweenExitMovement(amountMoved, Vector3.left);
                    break;
            }
        }
        else
        {
            // Without exit
            switch (direction)
            {
                case 0:
                    // Move amountMoved amount UP
                    TweenNoExitMovement(amountMoved, Vector3.forward);
                    _movable.Row -= amountMoved;
                    break;
                case 1:
                    // Move amountMoved amount RIGHT
                    TweenNoExitMovement(amountMoved, Vector3.right);
                    _movable.Col += amountMoved;
                    break;
                case 2:
                    // Move amountMoved amount DOWN
                    TweenNoExitMovement(amountMoved, Vector3.back);
                    _movable.Row += amountMoved;
                    break;
                case 3:
                    // Move amountMoved amount LEFT
                    TweenNoExitMovement(amountMoved, Vector3.left);
                    _movable.Col -= amountMoved;
                    break;
            }
            
            // Update the occupied cells
            UpdateOccupiedCells();
        }

        LevelManager.Instance.MadeMove();
    }

    private void TweenExitMovement(int amountMoved, Vector3 directionVector)
    {
        // Move the movableBehaviour until the exit, then scale down and shake while moving a bit further
        float beforeExitTweenTiming = 0.2f * amountMoved;
        float beforeExitTweenAdditionalMoveAmount = 0.25f * _movable.Length;
        Ease beforeExitMovementEase = Ease.OutSine;
            
        float afterExitTweenTiming = 1f;
        float afterExitTweenMoveAmount = 0.5f * _movable.Length;
        float afterExitTweenShakeStrength = 15f;
        
        transform.DOMove((amountMoved + beforeExitTweenAdditionalMoveAmount) * directionVector, beforeExitTweenTiming)
            .SetRelative()
            .SetEase(beforeExitMovementEase)
            .OnComplete(() =>
            {
                // After exit
                transform.DOMove(afterExitTweenMoveAmount * directionVector, afterExitTweenTiming)
                    .SetRelative();
                transform.DOScale(Vector3.zero, afterExitTweenTiming)
                    .SetEase(Ease.InBack);
                transform.DOShakeRotation(afterExitTweenTiming, afterExitTweenShakeStrength, 10);
            });
    }

    private void TweenNoExitMovement(int amountMoved, Vector3 directionVector)
    {
        float tweenDuration = 0.2f * amountMoved;
        Ease tweenEase = Ease.OutBack;
        
        transform.DOMove(amountMoved * directionVector, tweenDuration)
            .SetRelative()
            .SetEase(tweenEase);
    }
}
