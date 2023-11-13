using UnityEngine;

public class GridCell
{
    #region Properties

    public bool Taken { get; set; }
    public GameEntity GameEntity { get; set; }
    public int X { get; private set; }
    public int Y { get; private set; }

    #endregion

    #region Constructor

    public GridCell(int x, int y)
    {
        X = x;
        Y = y;
    }

    #endregion
}