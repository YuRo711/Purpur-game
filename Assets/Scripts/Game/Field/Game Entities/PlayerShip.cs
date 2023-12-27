using System;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShip : GameEntity
{
    #region Interactions
    
    private Action<GameEntity> _shootingInteraction;
    private Action<GameEntity> _teleportInteraction;

    #endregion

    #region SerializedProperties
    [field: SerializeField] public ActionMultiplier ActionMultiplier { get; private set; } = new ActionMultiplier();
    #endregion

    #region Audio Clips

    [SerializeField] private AudioClip movementClip;
    [SerializeField] private AudioClip turnClip;
    [SerializeField] private AudioClip shotClip;

    #endregion

    #region Public Methods

    public override void MoveInDirection(TurnDirections moveDir, int speed = 1)
    {
        base.MoveInDirection(moveDir, speed);
        soundManager.PlayAudioClip(movementClip);
    }

    public override void MoveTo(int destX, int destY, bool callSync = true, bool ignoreObjectCollision = false)
    {
        if (levelGrid.CheckForBorder(destX, destY))
            return;
        base.MoveTo(destX, destY, callSync);
        if (callSync)
            Debug.Log("player moved to " + X + " " + Y);
        if (enemyManager is not null)
            enemyManager.LookForPlayer();
    }

    public override void TurnTo(TurnDirections turnDirections, bool callSync = true)
    {
        base.TurnTo(turnDirections, callSync);
        soundManager.PlayAudioClip(turnClip);
    }

    public void Shoot(TurnDirections shootTurnDirection)
    {
        var shootAbsDirection = LookDirection.TurnTo(shootTurnDirection);
        moveVector = shootAbsDirection.TurnTo(TurnDirections.Around).Vector;
        MoveTo(X + (int)moveVector.x, Y + (int)moveVector.y);
        InteractWithFirst(shootTurnDirection, _shootingInteraction);
        soundManager.PlayAudioClip(shotClip);
    }

    public void Teleport(TurnDirections teleportTurnDirection)
    {
        InteractWithFirst(teleportTurnDirection, _teleportInteraction);
    }

    #endregion

    #region Private Methods

    private void InteractWithFirst(TurnDirections direction, Action<GameEntity> interaction)
    {
        var deltaVector = LookDirection.TurnTo(direction).Vector;
        var deltaX = (int)deltaVector.x;
        var deltaY = (int)deltaVector.y;
        var checkX = X + deltaX;
        var checkY = Y + deltaY;
        while (!levelGrid.CheckForBorder(checkX, checkY))
        {
            var cell = levelGrid.Cells[checkY, checkX];
            if (cell.GameEntity is GameEntity gameEntity && !gameEntity.isBackground)
            {
                interaction.Invoke(gameEntity);
                break;
            }
            checkX += deltaX;
            checkY += deltaY;
        }
        CallSync();
    }

    private void SwitchPlaces(GameEntity gameEntity)
    {
        var destX = gameEntity.X;
        var destY = gameEntity.Y;
        var oldX = X;
        var oldY = Y;
        DeleteFromCell(entityId);
        gameEntity.MoveTo(X, Y, true, true);
        MoveTo(destX, destY, true, true);
        levelGrid.Cells[oldY, oldX].GameEntity = gameEntity;
    }

    private void PushCargo(Cargo cargo)
    {
        var moveX = (int)moveVector.x;
        var moveY = (int)moveVector.y;
        var newCargoX = cargo.X + moveX;
        var newCargoY = cargo.Y + moveY;
        if (levelGrid.CheckForBorder(newCargoX, newCargoY))
        {
            cargo.TakeDamage(1);
            return;
        }
        cargo.MoveTo(newCargoX, newCargoY);
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
            {typeof(Cargo), e => PushCargo((Cargo)e)},
        };
        _shootingInteraction = entity => DamageEntity(entity, 1);
        _teleportInteraction = SwitchPlaces;
    }

    #endregion
}