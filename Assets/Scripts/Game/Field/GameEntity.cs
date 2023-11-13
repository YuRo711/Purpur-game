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
    public void Move(TurnDirections moveDir)
    {
        var newX = _x;
        var newY = _y;
        var moveVector = Direction.TurnTo(moveDir).Vector;
        newX += (int)moveVector.x;
        newY += (int)moveVector.y;

        if (newX < 0)
            newX = 0;
        if (newY < 0)
            newY = 0;
        if (newX > gameGrid.width - 1)
            newX = gameGrid.width - 1;
        if (newY > gameGrid.height - 1)
            newY = gameGrid.height - 1;
        
        MoveTo(newX, newY);
    }

    // For precise movement: warp, etc. Only use it if you're sure the cell exists.
    public void MoveTo(int destX, int destY)
    {
        _x = destX;
        _y = destX;
    }

    #endregion
}