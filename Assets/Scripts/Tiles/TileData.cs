using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData
{
    private Colors colorType;
    private int row;
    private int column;

    public int Row { get => row; set => row = value; }
    public int Column { get => column; set => column = value; }
    public Colors ColorType { get => colorType; set => colorType = value; }

    public TileData(int rowNew, int columnNew, Colors colorT)
    {
        row = rowNew;
        column = columnNew;
        colorType = colorT;
    }
}

public enum Colors
{
    red,
    green,
    blue,
    yellow,
    basic
}
