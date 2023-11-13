using UnityEngine;
using UnityEngine.Serialization;

public abstract class GameEntity : MonoBehaviour
{
    #region Serializable Fields

    private int _x;
    private int _y;

    #endregion
    
    #region Properties

    public Direction Direction { get; set; }
    [FormerlySerializedAs("grid")] public GameGrid gameGrid;

    #endregion

    #region Public Methods

    // For movement in some direction
    public void Move(TurnDirections moveDir)
    {
        int newX = _x;
        int newY = _y;
        
    }

    // For precise movement: warp, etc. Only use it if you're sure the cell exists.
    public void MoveTo(int destX, int destY)
    {
        _x = destX;
        _y = destX;
    }

    #endregion
}