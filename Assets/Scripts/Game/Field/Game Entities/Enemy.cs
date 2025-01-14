using System;
using Photon.Pun;
using UnityEngine;

public class Enemy : GameEntity
{
    #region SerializableFields

    [SerializeField] protected int lookX;
    [SerializeField] protected int lookY;

    #endregion
    
    #region Public Methods

    public void TakeAction()
    {
        MoveInDirection(TurnDirections.Forward);
    }

    public override void MoveTo(int destX, int destY, bool callSync = true, bool ignoreObjectCollision = false)
    {
        if (levelGrid is null)
        {
            SyncStart(entityId, destX, destY);
            return;
        }

        if (levelGrid.CheckForBorder(destX, destY))
        {
            TurnTo(TurnDirections.Around, callSync);
            return;
        }

        var newCell = levelGrid.Cells[destY, destX];
        if (newCell.GameEntity is not null && newCell.GameEntity is not PlayerShip
            || newCell.BgEntity is Gates)
        {
            TurnTo(TurnDirections.Around, callSync);
            return;
        }

        if (!ignoreObjectCollision)
        {
            var player = LevelManager.player;
            if (player.X == destX && player.Y == destY)
            {
                CollisionInteractions[typeof(PlayerShip)].Invoke(player);
                return;
            }

            CollideWithCellEntity(newCell);
        }

        if (levelGrid.Cells[Y, X].GameEntity is not null)
            levelGrid.Cells[Y, X].GameEntity = null;
        X = destX;
        Y = destY;
        AdaptTransform(newCell);
        newCell.GameEntity = this;
        LookForPlayer();
        if (callSync)
            CallSync();
    }

    public void LookForPlayer()
    {
        var y1 = Y;
        for (var x1 = 0; x1 < levelGrid.width; x1++)
        {
            var checkCell = levelGrid.Cells[y1, x1];
            if (checkCell.GameEntity is PlayerShip)
            {
                LookDirection = x1 < X ?
                    new Direction(-1, 0) :
                    new Direction(1, 0);
                TurnTo(TurnDirections.Forward, true);
                return;
            }
            if (checkCell.GameEntity is not null)
                if (x1 <= X)
                    x1 = X;
                else
                    break;
        }

        var x2 = X;
        for (var y2 = 0; y2 < levelGrid.height; y2++)
        {
            var checkCell = levelGrid.Cells[y2, x2];
            if (checkCell.GameEntity is PlayerShip)
            {
                LookDirection = y2 < Y ?
                    new Direction(0, -1) :
                    new Direction(0, 1);
                TurnTo(TurnDirections.Forward, true);
                return;
            }
            if (checkCell.GameEntity is not null)
                
                if (y2 <= Y)
                    y2 = Y;
                else
                    break;
        }
    }
    
    #endregion
    
    #region Protected Methods

    protected override void Die()
    {
        enemyManager.enemies.Remove(this);
        base.Die();
    }

    #endregion
    
    #region MonoBehaviour Callbacks
    
    private void Awake()
    {
        health = 1;
        
        CollisionInteractions = new()
        {
            {typeof(PlayerShip), e => DamageEntity(e, 1, 1)},
            {typeof(Cargo), e => DamageEntity(e, 0, 1)},
            {typeof(Enemy), e => DamageEntity(e, 1, 1)},
        };
    }

    #endregion
}

