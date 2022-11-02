using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Tile : MonoBehaviour
{
    private TileData data;
    private SpriteRenderer spriteRenderer;
    private GameManager gm;
    private StateManager stateManager;
    private InputHandler inputHandler;
    private Board board;
    private Coroutine movementCoroutine;

    public TileData Data { get => data; }
    public Board Board { get => board; }

    public void Init(int row, int column, Colors colorType, GameManager gameManager, Board boardValue)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        stateManager = gameManager.GetComponent<StateManager>();
        board = boardValue;
        gameObject.name = "Tile " + row.ToString() + " - " + column.ToString();
        data = new TileData(row, column, colorType);
        switch (data.ColorType)
        {
            case Colors.red:
                spriteRenderer.color = Constants.COLOR_RED;
                break;
            case Colors.green:
                spriteRenderer.color = Constants.COLOR_GREEN;
                break;
            case Colors.yellow:
                spriteRenderer.color = Constants.COLOR_YELLOW;
                break;
            case Colors.blue:
                spriteRenderer.color = Constants.COLOR_BLUE;
                break;
            default:
                spriteRenderer.color = Constants.COLOR_BASIC;
                break;
        }
        gm = gameManager;
        inputHandler = gameManager.GetComponent<InputHandler>();
    }

    private void OnMouseDown()
    {
        inputHandler.CleanClickPositions();
        if (stateManager.CurrentState != stateManager.statesList[Constants.STATE_ID_GAME] || movementCoroutine!= null) return;
        inputHandler.FirstPositionClick = Input.mousePosition;
        inputHandler.SelectedTile = this;
    }

    private void OnMouseUp()
    {
        if (stateManager.CurrentState != stateManager.statesList[Constants.STATE_ID_GAME] || movementCoroutine != null) return;
        inputHandler.LastPositionClick = Input.mousePosition;
        inputHandler.CheckDirection();
    }

    public void CheckForCombinations(Dictionary<Vector2, Tile> mapTiles)
    {
        if (data.Row - 1 >= 0 && data.ColorType == mapTiles[new Vector2(data.Row - 1, data.Column)].Data.ColorType)
        {
            if (data.Row + 1 < board.GameManager.Rows && data.ColorType == mapTiles[new Vector2(data.Row + 1, data.Column)].data.ColorType)
            {
                board.DestroyCombinationTiles(this, mapTiles[new Vector2(data.Row - 1, data.Column)], mapTiles[new Vector2(data.Row + 1, data.Column)]);
            }
            else if (data.Row - 2 >= 0 && data.ColorType == mapTiles[new Vector2(data.Row - 2, data.Column)].data.ColorType)
            {
                board.DestroyCombinationTiles(this, mapTiles[new Vector2(data.Row - 1, data.Column)], mapTiles[new Vector2(data.Row - 2, data.Column)]);
            }
        }
        if (data.Row + 2 < board.GameManager.Rows && data.ColorType == mapTiles[new Vector2(data.Row + 1, data.Column)].data.ColorType && data.ColorType == mapTiles[new Vector2(data.Row + 2, data.Column)].data.ColorType)
        {
            board.DestroyCombinationTiles(this, mapTiles[new Vector2(data.Row + 1, data.Column)], mapTiles[new Vector2(data.Row + 2, data.Column)]);
        }
        if (data.Column - 1 >= 0 && data.ColorType == mapTiles[new Vector2(data.Row, data.Column - 1)].data.ColorType)
        {
            if (data.Column + 1 < board.GameManager.Columns && data.ColorType == mapTiles[new Vector2(data.Row, data.Column + 1)].data.ColorType)
            {
                board.DestroyCombinationTiles(this, mapTiles[new Vector2(data.Row, data.Column - 1)], mapTiles[new Vector2(data.Row, data.Column + 1)]);
            }
            else if (data.Column - 2 >= 0 && data.ColorType == mapTiles[new Vector2(data.Row, data.Column - 2)].data.ColorType)
            {
                board.DestroyCombinationTiles(this, mapTiles[new Vector2(data.Row, data.Column - 1)], mapTiles[new Vector2(data.Row, data.Column - 2)]);
            }
        }
        if (data.Column + 2 < board.GameManager.Columns && data.ColorType == mapTiles[new Vector2(data.Row, data.Column + 1)].data.ColorType && data.ColorType == mapTiles[new Vector2(data.Row, data.Column + 2)].data.ColorType)
        {
            board.DestroyCombinationTiles(this, mapTiles[new Vector2(data.Row, data.Column + 1)], mapTiles[new Vector2(data.Row, data.Column + 2)]);
        }
    }

    public void MoveTile(Vector3 targetPosition)
    {
        movementCoroutine = StartCoroutine(MovingTile(targetPosition));
    }

    private IEnumerator MovingTile(Vector3 targetPosition)
    {
        var velocity = board.TransitionVelocity;
        while(Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, velocity);
            yield return new WaitForSeconds(0.02f);
        }
        StopCoroutine(movementCoroutine);
        movementCoroutine = null;
    }
}
