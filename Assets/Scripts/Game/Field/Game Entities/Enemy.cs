using System;
using UnityEngine;

public class Enemy : GameEntity
{
    #region Public Methods

    public void TakeAction()
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
        var rt = (RectTransform)transform;
        rt.SetParent(newCell.transform);
        rt.sizeDelta = new Vector2(size, size);
        rt.localPosition = Vector3.zero;
        newCell.GameEntity = this;
    }

    public void CheckForPlayer()
    {
        var y1 = y;
        for (var x1 = 0; x1 < LevelGrid.width; x1++)
        {
            if (LevelGrid.Cells[x1, y1].GameEntity is PlayerShip)
            {
                LookDirection = x1 < x ?
                    new Direction(-1, 0) :
                    new Direction(1, 0);
                return;
            }
        }

        var x2 = x;
        for (var y2 = 0; y2 < LevelGrid.height; y2++)
        {
            if (LevelGrid.Cells[x2, y2].GameEntity is PlayerShip)
            {
                LookDirection = y2 < y ?
                    new Direction(0, -1) :
                    new Direction(0, 1);
                return;
            }
        }
    }

    #endregion
}

