using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBehaviour : MonoBehaviour
{
    public Cell cellData;

    public void SetCell(Cell cellData)
    {
        this.cellData = cellData;
        
        // Positioning
        transform.Translate(new Vector3((cellData.Col - (LevelManager.Instance.levelData.ColCount / 2f) + 0.5f) * 2f,
                                                0f,
                                                (cellData.Row - (LevelManager.Instance.levelData.RowCount / 2f) + 0.5f) * 2f));
    }
}
