using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public abstract class GameEntity : MonoBehaviour
{
    #region Serializable Fields

    [SerializeField] protected int x;
    [SerializeField] protected int y;
    [SerializeField] protected int health;

    #endregion
    
    #region Properties

    public GameGrid LevelGrid;
    public Direction LookDirection { get; protected set; }

    #endregion

    
    #region Public Methods

    // For movement in some direction
    public virtual void MoveInDirection(TurnDirections moveDir, int speed = 1)
    {
        var newX = x;
        var newY = y;
        var moveVector = LookDirection.TurnTo(moveDir).Vector;
        newX += (int)moveVector.x * speed;
        newY += (int)moveVector.y * speed;

        newX = Math.Min(Math.Max(newX, 0), LevelGrid.width - 1);
        newY = Math.Min(Math.Max(newY, 0), LevelGrid.height - 1);
        
        MoveTo(newX, newY);
    }

    // For precise movement: warp, etc. Only use it if you're sure the cell exists.
    public virtual void MoveTo(int destX, int destY)
    {
        x = destX;
        y = destY;
    }

    public virtual void TurnTo(TurnDirections turnDirections)
    {
        LookDirection = LookDirection.TurnTo(turnDirections);
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Die();
    }

    #endregion

    #region Protected Methods

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    #endregion
}