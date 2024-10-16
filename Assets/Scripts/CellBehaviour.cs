using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBehaviour : MonoBehaviour
{
    public Cell cellData;
    public MovableBehaviour occupyingMovableBehaviour = null;
    public List<ExitBehaviour> attachedExitBehaviours = null;

    public void SetCell(Cell cellData)
    {
        this.cellData = cellData;
        
        // Positioning
        transform.Translate(new Vector3((cellData.Col - (LevelManager.Instance.levelData.ColCount / 2f) + 0.5f) * 1f,
                                                0f,
                                                -(cellData.Row - (LevelManager.Instance.levelData.RowCount / 2f) + 0.5f) * 1f));

        attachedExitBehaviours = new List<ExitBehaviour>();
    }

    public void SetOccupyingBlock(MovableBehaviour movableBehaviour)
    {
        if (movableBehaviour != null && occupyingMovableBehaviour != null) 
            Debug.LogWarning("Already occupied cell is occupied again");
        
        occupyingMovableBehaviour = movableBehaviour;
    }

    public bool IsOccupied()
    {
        return occupyingMovableBehaviour != null;
    }

    public void AddAttachedExitBehaviour(ExitBehaviour exitBehaviour)
    {
        attachedExitBehaviours.Add(exitBehaviour);
    }
}
