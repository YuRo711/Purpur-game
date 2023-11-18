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

    public override void MoveTo(int destX, int destY, bool callSync = true)
    {
        if (CheckForBorder(destX, destY))
        {
            TurnTo(TurnDirections.Around);
            return;
        }

        if (levelGrid.Cells[y, x].GameEntity is not null)
            levelGrid.Cells[y, x].GameEntity = null;
        var newCell = levelGrid.Cells[destY, destX];
        if (newCell.GameEntity is not null)
        {
            Debug.Log(newCell.GameEntity);
            if (newCell.GameEntity is PlayerShip playerShip)
            {
                playerShip.TakeDamage(1);
                Die();
                return;
            }
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
        LookForPlayer();
        if (callSync)
            CallSync();
    }

    public void LookForPlayer()
    {
        var y1 = y;
        for (var x1 = 0; x1 < levelGrid.width; x1++)
        {
            var checkCell = levelGrid.Cells[y1, x1];
            if (checkCell.GameEntity is PlayerShip)
            {
                LookDirection = x1 < x ?
                    new Direction(-1, 0) :
                    new Direction(1, 0);
                return;
            }
            if (checkCell.GameEntity is not null)
                if (x1 <= x)
                    x1 = x;
                else
                    break;
        }

        var x2 = x;
        for (var y2 = 0; y2 < levelGrid.height; y2++)
        {
            var checkCell = levelGrid.Cells[y2, x2];
            if (checkCell.GameEntity is PlayerShip)
            {
                LookDirection = y2 < y ?
                    new Direction(0, -1) :
                    new Direction(0, 1);
                return;
            }
            if (checkCell.GameEntity is not null)
                
                if (y2 <= y)
                    y2 = y;
                else
                    break;
        }
        CallSync();
    }

    public override void Die()
    {
        enemyManager.enemies.Remove(this);
        PhotonNetwork.Destroy(gameObject);
    }

    public override void SetStartParameters(int id)
    {
        base.SetStartParameters(id);
        enemyManager.enemies.Add(this);
    }

    #endregion
}

