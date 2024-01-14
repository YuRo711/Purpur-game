using System;

public class Gates : GameEntity
{
    #region MonoBehaviour Callbacks
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

        if (newCell.GameEntity is Cargo cargo)
        {
            cargo.EnterGates(this);
            return;
        }

        newCell.GameEntity = this;
        if (callSync)
            CallSync();
    }

    private void Awake()
    {
        isBackground = false;
        health = 1;
        CollisionInteractions = new()
        {
            {typeof(PlayerShip), e => DamageEntity(e, 1, 1)},
            {typeof(Enemy), e => DamageEntity(e, 1, 1)},
            {typeof(Signal), e => DamageEntity(e, 1, 1)},
            {typeof(Asteroid), e => DamageEntity(e, 1, 1)},
        };
    }

    #endregion
}