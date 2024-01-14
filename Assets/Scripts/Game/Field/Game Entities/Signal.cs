using System;
using Photon.Pun;
using UnityEngine;

public class Signal : GameEntity
{
    #region Public Methods
    
    public void ChangePosition()
    {
        var pos = levelGrid.GetRandomPosition();
        if (pos is null)
        {
            Die();
            levelGrid.OnEntityCannotSpawn();
            return;
        }
        MoveTo(pos.Item1, pos.Item2);
    }

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        health = 1;
    }

    #endregion
}