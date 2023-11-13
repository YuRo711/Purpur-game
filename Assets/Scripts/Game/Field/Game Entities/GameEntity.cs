using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public abstract class GameEntity : MonoBehaviour
{
    #region Serializable Fields

    private int _x;
    private int _y;

    #endregion
    
    #region Properties

    public Direction Direction { get; set; }
    public GameGrid gameGrid;

    #endregion

    
    #region Public Methods

    // For movement in some direction
    public void Move(TurnDirections moveDir, int speed = 1)
    {
        var newX = _x;
        var newY = _y;
        var moveVector = Direction.TurnTo(moveDir).Vector;
        newX += (int)moveVector.x * speed;
        newY += (int)moveVector.y * speed;

        newX = Math.Min(Math.Max(newX, 0), gameGrid.width - 1);
        newY = Math.Min(Math.Max(newY, 0), gameGrid.height - 1);
        
        MoveTo(newX, newY);
    }

    // For precise movement: warp, etc. Only use it if you're sure the cell exists.
    public void MoveTo(int destX, int destY)
    {
        _x = destX;
        _y = destX;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    #endregion
}