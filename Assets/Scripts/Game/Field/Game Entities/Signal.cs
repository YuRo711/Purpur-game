using System;
using Photon.Pun;
using UnityEngine;

public class Signal : GameEntity
{
    #region Public Methods
    
    public bool TryChangePosition()
    {
        var pos = levelGrid.GetRandomPosition();
        if (pos is null)
        {
            //Die();
            levelGrid.OnEntityCannotSpawn();
            return false;
        }
        MoveTo(pos.Item1, pos.Item2);
        return true;
    }

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        health = 1;
    }

    #endregion
}