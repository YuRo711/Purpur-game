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
        MoveTo(pos.Item1, pos.Item2);
    }
    
    [PunRPC]
    public override void SyncStart(int id)
    {
        base.SyncStart(id);
        if (entityId != id)
            return;
        isBackground = true;
    }

    #endregion
}