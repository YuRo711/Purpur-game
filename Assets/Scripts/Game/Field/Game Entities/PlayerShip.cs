using System;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShip : GameEntity
{
    #region Interactions
    
    protected Dictionary<Type, Action<GameEntity>> ShootingInteractions;

    private void PickUpCargo(Cargo cargo)
    {
        
    }

    private void EnterGates(Gates gates)
    {
        
    }

    #endregion
    
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
        if (levelGrid.CheckForBorder(destX, destY))
            return;
        levelGrid.Cells[y, x].GameEntity = null;
        x = destX;
        y = destY;
        var newCell = levelGrid.Cells[y, x];
        AdaptTransform(newCell);
        GetInteraction(newCell, CollisionInteractions);
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
        while (!levelGrid.CheckForBorder(checkX, checkY))
        {
            var cell = levelGrid.Cells[checkY, checkX];
            var isShot = GetInteraction(cell, ShootingInteractions);
            if (isShot is not null)
            {
                InteractWithCellEntity(cell, ShootingInteractions);
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
        CollisionInteractions = new()
        {
            {typeof(Enemy), e => DamageEntity(e, 1, 1)},
            {typeof(Asteroid), e => DamageEntity(e, 1, 1)},
            {typeof(Cargo), e => PickUpCargo((Cargo)e)},
            {typeof(Gates), e => EnterGates((Gates)e)},
        };
        ShootingInteractions = new()
        {
            { typeof(Enemy), e => DamageEntity(e, 1) },
            { typeof(Asteroid), e => DamageEntity(e, 1) },
            { typeof(Cargo), e => DamageEntity(e, 1) },
        };
    }

    #endregion
}