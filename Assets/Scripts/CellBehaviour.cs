using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBehaviour : MonoBehaviour
{
    public Cell cellData;
    public Block occupyingBlock = null;

    public void SetCell(Cell cellData)
    {
        this.cellData = cellData;
        
        // Positioning
        transform.Translate(new Vector3((cellData.Col - (LevelManager.Instance.levelData.ColCount / 2f) + 0.5f) * 1f,
                                                0f,
                                                -(cellData.Row - (LevelManager.Instance.levelData.RowCount / 2f) + 0.5f) * 1f));
    }

    public void SetOccupyingBlock(Block block)
    {
        if (block != null && occupyingBlock != null) 
            Debug.LogWarning("Already occupied cell is occupied again");
        
        occupyingBlock = block;
    }

    public bool IsOccupied()
    {
        return occupyingBlock != null;
    }
}
