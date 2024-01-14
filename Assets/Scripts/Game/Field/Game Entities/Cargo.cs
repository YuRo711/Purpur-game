using Photon.Pun;
using UnityEngine;

public class Cargo : GameEntity
{
    #region Public Methods
    
    public override void MoveTo(int destX, int destY, bool callSync = true, bool ignoreObjectCollision = false)
    {
        if (levelGrid.CheckForBorder(destX, destY))
            return;
        levelGrid.Cells[Y, X].GameEntity = null;
        X = destX;
        Y = destY;
        var newCell = levelGrid.Cells[Y, X];
        AdaptTransform(newCell);
        CollideWithCellEntity(newCell);
        if (newCell.GameEntity is Gates gates)
        {
            EnterGates(gates);
            return;
        }

        newCell.GameEntity = this;
        if (callSync)
            CallSync();
    }

    #endregion

    #region Private Methods

    public void EnterGates(Gates gates)
    {
        LevelManager.RequestScoreIncrease();
        TakeDamage(1);
        gates.TakeDamage(1);
    }
    
    #endregion
    
    #region MonoBehaviour Callbacks
    
    private void Awake()
    {
        health = 1;
        CollisionInteractions = new()
        {
            {typeof(Enemy), e => DamageEntity(e, 1, 1)},
            {typeof(Asteroid), e => DamageEntity(e, 1, 1)},
            {typeof(Cargo), e => DamageEntity(e, 1, 1)},
            {typeof(PlayerShip), e => DamageEntity(e, 1, 1)},
            {typeof(Signal), e => DamageEntity(e, 1, 1)}
        };
    }

    #endregion
}