using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool isActive;
    public Movable movable;

    [SerializeField] private Texture[] colorUpDownTextures = default;
    [SerializeField] private Texture[] colorRightLeftTextures = default;

    public void SetMovable(Movable movable)
    {
        this.movable = movable;
        
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
        
        // Register this Block to the cell(s) it occupies
        LevelManager.Instance.cellBehaviours[movable.Row, movable.Col].SetOccupyingBlock(this);
        if (movable.Length > 1)
        {
            // If the length is > 1, register this block to all the other cells it occupies
            for (int i = 1; i < movable.Length; i++)
            {
                if (movable.Direction[0] == 0 || movable.Direction[1] == 0)
                {
                    // Go up
                    LevelManager.Instance.cellBehaviours[movable.Row + i, movable.Col].SetOccupyingBlock(this);
                }
                else
                {
                    // Go right
                    LevelManager.Instance.cellBehaviours[movable.Row, movable.Col + i].SetOccupyingBlock(this);
                }
            }
        }
        
    }
}
