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

    public Direction(float x, float y)
    {
        Vector = new Vector2(x, y);
    }

    #endregion

    #region Public Methods

    public Direction TurnTo(TurnDirections turn)
    {
        switch (turn)
        {
            case TurnDirections.Left:
                return new Direction(-Vector.y, Vector.x);
            case TurnDirections.Right:
                return new Direction(Vector.y, -Vector.x);
            case TurnDirections.Around:
                return new Direction(-Vector.x, -Vector.y);
        }
        return this;
    }
    
    #endregion
}