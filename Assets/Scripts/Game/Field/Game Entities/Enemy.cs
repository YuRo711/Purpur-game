using System;
using UnityEngine;

public class Enemy : GameEntity
{
    #region Public Methods

    public void TakeAction()
    {
        var lookVector = LookDirection.Vector;
        var nextCell = LevelGrid.Cells[x + (int)lookVector.x, y + (int)lookVector.y];
        if (nextCell.GameEntity is PlayerShip playerShip)
        {
            playerShip.TakeDamage(1);
            Die();
            return;
        }
        if (nextCell.GameEntity is not null)
            TurnTo(TurnDirections.Around);
        if (nextCell.GameEntity is null)
            MoveInDirection(TurnDirections.Forward);
    }

    #endregion

    #region MonoBehaviour Callbacks

    private void Start()
    {
        LookDirection = new Direction(1, 0);
        TakeAction();
    }

    #endregion
}

