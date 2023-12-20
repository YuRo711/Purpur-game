using System;
using Photon.Pun;
using UnityEngine;

public class Signal : GameEntity
{
    #region Public Methods

    public void UpdateSignal()
    {
        levelGrid.SpawnRandomSpawnable(X, Y);
        ChangePosition();
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
        isBackground = true;
    }

    #endregion
}