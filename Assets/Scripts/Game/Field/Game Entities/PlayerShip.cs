using System;
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
        x = destX;
        y = destY;
        var newCell = LevelGrid.Cells[x, y];
        var rt = (RectTransform)transform;
        rt.SetParent(newCell.transform);
        rt.sizeDelta = new Vector2(size, size);
        rt.localPosition = Vector3.zero;
        if (newCell.GameEntity is GameEntity gameEntity)
        {
            gameEntity.Die();
            TakeDamage(1);
        }
    }

    public void Shoot()
    {
        var checkX = x;
        var checkY = y;
        var lookVector = LookDirection.Vector;
        var lookX = (int)lookVector.x;
        var lookY = (int)lookVector.y;
        while (CheckForBorder(checkX, checkY))
        {
            checkX += lookX;
            checkY += lookY;
            if (LevelGrid.Cells[checkX, checkY].GameEntity is GameEntity gameEntity)
            {
                gameEntity.TakeDamage(1);
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