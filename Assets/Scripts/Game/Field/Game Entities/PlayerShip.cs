using System;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShip : GameEntity
{
    #region Public Methods

    public override void TurnTo(TurnDirections turnDirections)
    {
        base.TurnTo(turnDirections);
        var angle = LookDirection.GetDirectionAngle();
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public override void MoveTo(int destX, int destY)
    {
        if (CheckForBorder(destX, destY))
            return;
        LevelGrid.Cells[y, x].GameEntity = null;
        x = destX;
        y = destY;
        var newCell = LevelGrid.Cells[y, x];
        var rt = (RectTransform)transform;
        rt.SetParent(newCell.transform);
        rt.sizeDelta = new Vector2(size, size);
        rt.localPosition = Vector3.zero;
        if (newCell.GameEntity is not null)
        {
            newCell.GameEntity.Die();
            TakeDamage(1);
        }
        newCell.GameEntity = this;
        enemyManager.LookForPlayer();
    }

    public void Shoot(TurnDirections shootTurnDirection, int shotPower = 1)
    {
        var checkX = x;
        var checkY = y;
        var shootAbsDirection = LookDirection.TurnTo(shootTurnDirection);
        var shootVector = shootAbsDirection.Vector;
        var shootX = (int)shootVector.x;
        var shootY = (int)shootVector.y;
        var moveVector = shootAbsDirection.TurnTo(TurnDirections.Around).Vector;
        MoveTo(x + (int)moveVector.x, y + (int)moveVector.y);
        while (!CheckForBorder(checkX, checkY))
        {
            checkX += shootX;
            checkY += shootY;
            if (LevelGrid.Cells[checkY, checkX].GameEntity is GameEntity gameEntity)
            {
                gameEntity.TakeDamage(shotPower);
                break;
            }
        }
    }

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        LookDirection = new Direction(0, 1);
    }

    #endregion
}