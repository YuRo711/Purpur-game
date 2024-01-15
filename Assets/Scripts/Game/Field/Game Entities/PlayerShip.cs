using System;
using System.Collections.Generic;
using Assets.Scripts.Game.Control_Panel;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShip : GameEntity
{
    #region Interactions
    
    private Action<GameEntity> _shootingInteraction;
    private Action<GameEntity> _teleportInteraction;

    #endregion

    #region Properties
    [field: SerializeField] public ButtonDeck ButtonDeck { get; private set; }
    [field: SerializeField] public ChargeManager ChargeManager{ get; private set; }

    [field: SerializeField] public bool IsImmortal { get; private set; }
    #endregion

    #region Audio Clips

    [SerializeField] protected string deathClip;
    [SerializeField] private string movementClip;
    [SerializeField] private string turnClip;
    [SerializeField] private string shotClip;
    [SerializeField] private string teleportClip;

    #endregion

    #region Public Methods

    public override void MoveInDirection(TurnDirections moveDir, int speed = 1)
    {
        base.MoveInDirection(moveDir, speed);
        PlayAudioClip(movementClip);
    }

    public override void MoveTo(int destX, int destY, bool callSync = true, bool ignoreObjectCollision = false)
    {
        if (levelGrid.CheckForBorder(destX, destY))
            return;
        base.MoveTo(destX, destY, callSync);
        if (enemyManager is not null)
            enemyManager.LookForPlayer();
    }

    public override void TurnTo(TurnDirections turnDirections, bool callSync = true)
    {
        base.TurnTo(turnDirections, callSync);
        if (callSync)
            PlayAudioClip(turnClip);
    }

    public void Shoot(TurnDirections shootTurnDirection)
    {
        var shootAbsDirection = LookDirection.TurnTo(shootTurnDirection);
        moveVector = shootAbsDirection.TurnTo(TurnDirections.Around).Vector;
        MoveTo(X + (int)moveVector.x, Y + (int)moveVector.y);
        var targetCell = FindFirstEntityOrLastCell(shootTurnDirection);
        if(targetCell.GameEntity is GameEntity gameEntity)
        {
            _shootingInteraction.Invoke(gameEntity);
        }
        CallSync();
        PlayAudioClip(shotClip);
    }

    public void Teleport(TurnDirections teleportTurnDirection)
    {
        var targetCell = FindFirstEntityOrLastCell(teleportTurnDirection);

        if (targetCell == null)
        {
            PlayAudioClip(teleportClip);
            return;
        }

        if(targetCell.GameEntity is GameEntity gameEntity)
        {
            _teleportInteraction(gameEntity);
        }
        else
        {
            MoveTo(targetCell.X, targetCell.Y, true, true);
        }
        CallSync();
        PlayAudioClip(teleportClip);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (health > 0)
            MoveTo(X, Y);
    }

    #endregion

    #region Private Methods

    private GridCell FindFirstEntityOrLastCell(TurnDirections direction)
    {
        var deltaVector = LookDirection.TurnTo(direction).Vector;
        var deltaX = (int)deltaVector.x;
        var deltaY = (int)deltaVector.y;
        var checkX = X + deltaX;
        var checkY = Y + deltaY;
        GridCell lastValidCell = null;

        while (!levelGrid.CheckForBorder(checkX, checkY))
        {
            var cell = levelGrid.Cells[checkY, checkX];
            lastValidCell = cell;

            if (cell.GameEntity is GameEntity gameEntity && !gameEntity.isBackground)
            {
                return cell;
            }

            checkX += deltaX;
            checkY += deltaY;
        }

        return lastValidCell;
    }

    private void SwitchPlaces(GameEntity gameEntity)
    {
        var destX = gameEntity.X;
        var destY = gameEntity.Y;
        var oldX = X;
        var oldY = Y;
        photonView.RPC("DeleteFromCell", RpcTarget.AllBuffered, entityId, false);
        gameEntity.MoveTo(X, Y, true, true);
        MoveTo(destX, destY, true, true);
        levelGrid.Cells[oldY, oldX].GameEntity = gameEntity;
    }

    private void PushEntity(GameEntity entity)
    {
        var moveX = (int)moveVector.x;
        var moveY = (int)moveVector.y;
        var newCargoX = entity.X + moveX;
        var newCargoY = entity.Y + moveY;
        if (levelGrid.CheckForBorder(newCargoX, newCargoY))
        {
            entity.TakeDamage(1);
            return;
        }
        entity.MoveTo(newCargoX, newCargoY);
    }

    protected override void Die()
    {
        base.Die();
        PlayAudioClip(deathClip);
        LevelManager.FinishGame();
    }

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        if (IsImmortal)
            health = 999999;
        else
            health = 1;

        collisionClip = "";

        CollisionInteractions = new()
        {
            {typeof(Enemy), e => DamageEntity(e, 1, 1)},
            {typeof(Asteroid), e => DamageEntity(e, 1, 1)},
            {typeof(Signal), e => DamageEntity(e, 1, 1)},
            {typeof(Cargo), e => PushEntity(e)},
            {typeof(Gates), e => PushEntity(e)},
        };
        _shootingInteraction = entity => DamageEntity(entity, 1);
        _teleportInteraction = SwitchPlaces;
    }

    #endregion
}