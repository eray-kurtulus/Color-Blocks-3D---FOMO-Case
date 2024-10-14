[System.Serializable]
public class LevelData
{
    public int MoveLimit;
    public int RowCount;
    public int ColCount;
    public Cell[] CellInfo;
    public Movable[] MovableInfo;
    public Exit[] ExitInfo;
}

[System.Serializable]
public class Cell
{
    public int Row;
    public int Col;
}

[System.Serializable]
public class Movable
{
    public int Row;
    public int Col;
    public int[] Direction;
    public int Length;
    public int Colors;
}

[System.Serializable]
public class Exit
{
    public int Row;
    public int Col;
    public int Direction;
    public int Colors;
}
