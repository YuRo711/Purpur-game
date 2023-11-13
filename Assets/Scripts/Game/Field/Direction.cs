using System;
using Unity.VisualScripting;
using UnityEngine;

public class Direction
{
    #region Properties

    public Vector2 Vector
    {
        get => _vector;
        set
        {
            if (value.x % 1 != 0 || value.y % 1 != 0)
                throw new ArgumentException(
                    "Coordinates should be whole numbers, you silly goose");
            if (Math.Abs(value.x) > 1 || Math.Abs(value.y) > 1)
                throw new ArgumentException(
                    "You can't set coordinates to anything but 0, 1 and -1, dummy");
            _vector = value;
        }
    }
    private Vector2 _vector;

    #endregion

    #region Constructor

    public Direction(int x, int y)
    {
        Vector = new Vector2(x, y);
    }

    #endregion

    #region Public Methods

    public void TurnTo(TurnDirections turn)
    {
        switch (turn)
        {
            case TurnDirections.Left:
                Vector = new Vector2(- Vector.y, Vector.x);
                break;
            case TurnDirections.Right:
                Vector = new Vector2(Vector.y, -Vector.x);
                break;
            case TurnDirections.Around:
                Vector = new Vector2(-Vector.x, -Vector.y);
                break;
        }
    }
    
    #endregion
}