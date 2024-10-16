using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitBehaviour : MonoBehaviour
{
    public Exit exitData;

    [SerializeField] private Animator _animator = default;
    [SerializeField] private Color[] _colors = default;

    private int _animHide = Animator.StringToHash("Hide");

    public void SetExitData(Exit exitData)
    {
        this.exitData = exitData;
        
        // Positioning and rotation
        transform.Translate(new Vector3((exitData.Col - (LevelManager.Instance.levelData.ColCount / 2f) + 0.5f) * 1f,
                                                0f,
                                                -(exitData.Row - (LevelManager.Instance.levelData.RowCount / 2f) + 0.5f) * 1f));

        switch (exitData.Direction)
        {
            case 0:
                // transform.rotation = Quaternion.Euler(); // No rotation
                break;
            case 1:
                transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                break;
            case 2:
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                break;
            case 3:
                transform.rotation = Quaternion.Euler(0f, 270f, 0f);
                break;
        }
        
        // Set material color according to Exit.Colors
        // TODO Setting both gates to the same color for now,
        // TODO because this case doesn't involve multiple colors for exits
        transform.GetChild(0).GetComponent<MeshRenderer>().material.color = _colors[exitData.Colors];
        transform.GetChild(1).GetComponent<MeshRenderer>().material.color = _colors[exitData.Colors];
        
        // Attach this to the cell
        LevelManager.Instance.cellBehaviours[exitData.Row, exitData.Col].AddAttachedExitBehaviour(this);
    }

    public void HideAnimation()
    {
        _animator.SetTrigger(_animHide);
    }
}
