using System;
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

    public void Shoot()
    {
        
    }

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        LookDirection = new Direction(0, 1);
    }

    #endregion
}