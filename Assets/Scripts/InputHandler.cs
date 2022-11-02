using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private Vector3 firstPositionClick;
    private Vector3 lastPositionClick;
    [SerializeField] private Tile selectedTile;

    public Tile SelectedTile { get => selectedTile; set => selectedTile = value; }
    public Vector3 FirstPositionClick { get => firstPositionClick; set => firstPositionClick = value; }
    public Vector3 LastPositionClick { get => lastPositionClick; set => lastPositionClick = value; }

    public void CheckDirection()
    {
        var direction = lastPositionClick - firstPositionClick;
        if (direction == Vector3.zero) return;
        if(Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
        {
            if(direction.x > 0)
            {
                selectedTile.Board?.MoveTileData(MovementDirection.right, selectedTile);
            }
            else
            {
                selectedTile.Board?.MoveTileData(MovementDirection.left, selectedTile);
            }
        }
        else
        {
            if (direction.y > 0)
            {
                selectedTile.Board?.MoveTileData(MovementDirection.up, selectedTile);
            }
            else
            {
                selectedTile.Board?.MoveTileData(MovementDirection.down, selectedTile);
            }
        }
    }

    public void CleanClickPositions()
    {
        firstPositionClick = Vector3.zero;
        lastPositionClick = Vector3.zero;
        selectedTile = null;
    }

}
public enum MovementDirection
{
    left,
    right,
    up,
    down
}
