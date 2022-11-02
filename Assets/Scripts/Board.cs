using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private Grid grid;
    [SerializeField] private Tile tilePrefab;
    private Dictionary<Vector2, Tile> mapTiles = new Dictionary<Vector2, Tile>();
    private GameManager _gameManager;
    private Tile otherTileMoved;
    [SerializeField] private float transitionVelocity;

    public float TransitionVelocity { get => transitionVelocity; }
    public GameManager GameManager { get => _gameManager; }

    public void Init(GameManager gm)
    {
        _gameManager = gm;
        _gameManager.preGameStart += CreateGrid;
        _gameManager.reset += ResetGame;
    }

    public void CreateGrid()
    {
        grid = GetComponent<Grid>();
        int columns = _gameManager.Columns;
        int rows = _gameManager.Rows;
        for (int column = 0; column < columns; column++)
        {
            for (int row = 0; row < rows; row++)
            {
                var tile = Instantiate(tilePrefab, new Vector3(column * (grid.cellSize.x + grid.cellGap.x), row * (grid.cellSize.y + grid.cellGap.y), 0), Quaternion.identity, transform);
                tile.Init(row, column, Colors.basic, _gameManager, this);
                mapTiles.Add(new Vector2(row, column), tile);
            }
        }
        for (int column = 0; column < columns; column++)
        {
            for(int row = 0; row < rows; row++)
            {
                var colorType = ChooseTileColorTypeSpawn();
                while (!CheckCanTileSpawn(colorType, row, column))
                {
                    colorType = ChooseTileColorTypeSpawn();
                }
                mapTiles[new Vector2(row, column)].Init(row, column, colorType, _gameManager, this);
            }
        }
        CenteringCamera(rows, columns);
    }

    private void CenteringCamera(int rows, int columns)
    {
        Camera.main.transform.position = new Vector3(columns * (grid.cellSize.y - grid.cellGap.y) / 2f, rows * (grid.cellSize.x - grid.cellGap.x) / 2f, -10f);
    }

    private Colors ChooseTileColorTypeSpawn()
    {
        var colorTypeInt = Random.Range(0, typeof(Colors).GetEnumNames().Length - 1);
        var colorType = (Colors)colorTypeInt;
        return colorType;
    }

    private bool CheckCanTileSpawn(Colors colorT, int row, int column)
    {
        if (row - 1 >= 0 && colorT == mapTiles[new Vector2(row - 1, column)].Data.ColorType)
        {
            if (row + 1 < _gameManager.Rows && colorT == mapTiles[new Vector2(row + 1, column)].Data.ColorType)
            {
                return false;
            }
            else if (row - 2 >= 0 && colorT == mapTiles[new Vector2(row - 2, column)].Data.ColorType)
            {
                return false;
            }
        }
        if (row + 2 < _gameManager.Rows && colorT == mapTiles[new Vector2(row + 1, column)].Data.ColorType && colorT == mapTiles[new Vector2(row + 2, column)].Data.ColorType)
        {
            return false;
        }
        if (column - 1 >= 0 && colorT == mapTiles[new Vector2(row, column - 1)].Data.ColorType)
        {
            if (column + 1 < _gameManager.Columns && colorT == mapTiles[new Vector2(row, column + 1)].Data.ColorType)
            {
                return false;
            }
            else if (column - 2 >= 0 && colorT == mapTiles[new Vector2(row, column - 2)].Data.ColorType)
            {
                return false;
            }
        }
        if (column + 2 < _gameManager.Columns && colorT == mapTiles[new Vector2(row, column + 1)].Data.ColorType && colorT == mapTiles[new Vector2(row, column + 2)].Data.ColorType)
        {
            return false;
        }
        return true;
    }

    public void MoveTileData(MovementDirection direction, Tile tile)
    {
        switch (direction)
        {
            case MovementDirection.right:
                if (tile.Data.Column + 1 >= _gameManager.Columns) return;
                otherTileMoved = mapTiles[new Vector2(tile.Data.Row, tile.Data.Column + 1)];
                otherTileMoved.Data.Column -= 1;
                tile.Data.Column += 1;
                break;
            case MovementDirection.left:
                if (tile.Data.Column - 1 < 0) return;
                otherTileMoved = mapTiles[new Vector2(tile.Data.Row, tile.Data.Column - 1)];
                otherTileMoved.Data.Column += 1;
                tile.Data.Column -= 1;
                break;
            case MovementDirection.up:
                if (tile.Data.Row + 1 >= _gameManager.Rows) return;
                otherTileMoved = mapTiles[new Vector2(tile.Data.Row + 1, tile.Data.Column)];
                otherTileMoved.Data.Row -= 1;
                tile.Data.Row += 1;
                break;
            case MovementDirection.down:
                if (tile.Data.Row - 1 < 0) return;
                otherTileMoved = mapTiles[new Vector2(tile.Data.Row - 1, tile.Data.Column)];
                otherTileMoved.Data.Row += 1;
                tile.Data.Row -= 1;
                break;
        }
        mapTiles[new Vector2(otherTileMoved.Data.Row, otherTileMoved.Data.Column)] = otherTileMoved;
        mapTiles[new Vector2(tile.Data.Row, tile.Data.Column)] = tile;
        MoveTiles(tile, otherTileMoved);
        tile.CheckForCombinations(mapTiles);
        otherTileMoved.CheckForCombinations(mapTiles);
        return;
    }

    private void MoveTiles(Tile firstTile, Tile secondTile)
    {
        var targetFirstTilePosition = secondTile.transform.position;
        var targetSecondTilePosition = firstTile.transform.position;

        firstTile.MoveTile(targetFirstTilePosition);
        secondTile.MoveTile(targetSecondTilePosition);
    }

    public void DestroyCombinationTiles(Tile first, Tile second, Tile third)
    {
        Tile[] tiles = {first, second, third};
        ReplaceTile(tiles);
        Destroy(first.gameObject, .1f);
        Destroy(second.gameObject, .1f);
        Destroy(third.gameObject, .1f);
        _gameManager.combinationMade(first.Data.ColorType);
    }

    private void ReplaceTile(Tile[] tiles)
    {
        for(int i = 0; i < tiles.Length; i++)
        {
            var tile = Instantiate(tilePrefab, new Vector3(tiles[i].Data.Column * (grid.cellSize.x + grid.cellGap.x), tiles[i].Data.Row * (grid.cellSize.y + grid.cellGap.y), 0), Quaternion.identity, transform);
            mapTiles[new Vector2(tiles[i].Data.Row, tiles[i].Data.Column)] = tile;
            var colorType = ChooseTileColorTypeSpawn();
            while (!CheckCanTileSpawn(colorType, tiles[i].Data.Row, tiles[i].Data.Column))
            {
                colorType = ChooseTileColorTypeSpawn();
            }
            tile.Init(tiles[i].Data.Row, tiles[i].Data.Column, colorType, _gameManager, this);
        }
    }

    private void ResetGame()
    {
        foreach(var tile in mapTiles.Values)
        {
            Destroy(tile.gameObject);
        }
        mapTiles.Clear();
    }
}
