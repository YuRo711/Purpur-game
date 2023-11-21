using System;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShip : GameEntity
{
    #region Interactions
    
    protected Action<GameEntity> ShootingInteraction;
    protected Action<GameEntity> TeleportInteraction;

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
        CollideWithCellEntity(newCell);
        newCell.GameEntity = this;
        if (enemyManager is not null)
            enemyManager.LookForPlayer();
        
        if (callSync)
            CallSync();
    }

    public void Shoot(TurnDirections shootTurnDirection)
    {
        var shootAbsDirection = LookDirection.TurnTo(shootTurnDirection);
        var moveVector = shootAbsDirection.TurnTo(TurnDirections.Around).Vector;
        MoveTo(x + (int)moveVector.x, y + (int)moveVector.y);
        InteractWithFirst(shootTurnDirection, ShootingInteraction);
    }

    #endregion

    #region Private Methods

    private void InteractWithFirst(TurnDirections direction, Action<GameEntity> interaction)
    {
        var deltaVector = LookDirection.TurnTo(direction).Vector;
        var deltaX = (int)deltaVector.x;
        var deltaY = (int)deltaVector.y;
        var checkX = x + deltaX;
        var checkY = y + deltaY;
        while (!levelGrid.CheckForBorder(checkX, checkY))
        {
            var cell = levelGrid.Cells[checkY, checkX];
            if (cell.GameEntity is GameEntity gameEntity && !gameEntity.IsBackground)
                interaction.Invoke(gameEntity);
            checkX += deltaX;
            checkY += deltaY;
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
        ShootingInteraction = entity => DamageEntity(entity, 1);
    }

    #endregion
}