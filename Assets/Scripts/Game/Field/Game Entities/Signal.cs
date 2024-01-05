using System;
using Photon.Pun;
using UnityEngine;

public class Signal : GameEntity
{
    #region Public Methods

    public void UpdateSignal()
    {
        var oldX = X;
        var oldY = Y;
        ChangePosition();
        levelGrid.SpawnRandomSpawnable(oldX, oldY);
    }
    
    private void ChangePosition()
    {
        var pos = levelGrid.GetRandomPosition();
        if (pos is null)
            return;
        MoveTo(pos.Item1, pos.Item2);
    }

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        health = 1;
        CollisionInteractions = new()
        {
            {typeof(PlayerShip), e => DamageEntity(e, 1, 1)},
            {typeof(Enemy), e => DamageEntity(e, 1, 1)},
        };
    }

    #endregion
}