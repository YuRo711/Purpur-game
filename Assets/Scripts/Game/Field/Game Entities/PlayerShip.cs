using System;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShip : GameEntity
{
    #region Public Methods

    public override void TurnTo(TurnDirections turnDirections, bool callSync = true)
    {
        base.TurnTo(turnDirections, callSync);
        var angle = LookDirection.GetDirectionAngle();
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        if (callSync)
            CallSync();
    }

    public override void MoveTo(int destX, int destY, bool callSync = true)
    {
        Debug.LogError("moving to " + destX + " " + destY);
        if (CheckForBorder(destX, destY))
            return;
        levelGrid.Cells[y, x].GameEntity = null;
        x = destX;
        y = destY;
        var newCell = levelGrid.Cells[y, x];
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
        if (enemyManager is not null)
            enemyManager.LookForPlayer();
        
        if (callSync)
            CallSync();
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
        checkX += shootX;
        checkY += shootY;
        while (!CheckForBorder(checkX, checkY))
        {
            if (levelGrid.Cells[checkY, checkX].GameEntity is GameEntity gameEntity)
            {
                gameEntity.TakeDamage(shotPower);
                break;
            }
            checkX += shootX;
            checkY += shootY;
        }
        CallSync();
    }

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        health = 3;
    }

    #endregion
}