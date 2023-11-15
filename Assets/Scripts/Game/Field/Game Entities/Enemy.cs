using System;
using UnityEngine;

public class Enemy : AutoEntity
{
    #region Public Methods

    public override void TakeAction()
    {
        MoveInDirection(TurnDirections.Forward);
    }

    public override void MoveTo(int destX, int destY)
    {
        if (CheckForBorder(destX, destY))
        {
            TurnTo(TurnDirections.Around);
            return;
        }
        
        var newCell = LevelGrid.Cells[destX, destY];
        if (newCell.GameEntity is PlayerShip playerShip)
        {
            playerShip.TakeDamage(1);
            Die();
            return;
        }
        if (newCell.GameEntity is GameEntity gameEntity)
        {
            TurnTo(TurnDirections.Around);
            return;
        }
        x = destX;
        y = destY;
    }

    #endregion
}

