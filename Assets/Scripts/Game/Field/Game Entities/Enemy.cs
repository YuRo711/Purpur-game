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

    public override void MoveTo(int destX, int destY)
    {
        if (CheckForBorder(destX, destY))
        {
            TurnTo(TurnDirections.Around);
            return;
        }

        if (LevelGrid.Cells[y, x].GameEntity is not null)
            LevelGrid.Cells[y, x].GameEntity = null;
        var newCell = LevelGrid.Cells[destY, destX];
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
    }

    public void LookForPlayer()
    {
        var y1 = y;
        for (var x1 = 0; x1 < LevelGrid.width; x1++)
        {
            var checkCell = LevelGrid.Cells[y1, x1];
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
        for (var y2 = 0; y2 < LevelGrid.height; y2++)
        {
            var checkCell = LevelGrid.Cells[y2, x2];
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
    }

    public override void Die()
    {
        enemyManager.enemies.Remove(this);
        Destroy(gameObject);
    }

    #endregion

    #region MonoBehaviour Callbacks

    protected void Awake()
    {
        LookDirection = new Direction(lookX, lookY);
    }

    protected override void Start()
    {
        base.Start();
        enemyManager.enemies.Add(this);
    }

    #endregion
}

