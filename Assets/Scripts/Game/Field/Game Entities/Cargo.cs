public class Cargo : GameEntity
{
    #region Public Methods
    
    public override void MoveTo(int destX, int destY, bool callSync = true)
    {
        if (levelGrid.CheckForBorder(destX, destY))
            return;
        levelGrid.Cells[Y, X].GameEntity = null;
        X = destX;
        Y = destY;
        var newCell = levelGrid.Cells[Y, X];
        AdaptTransform(newCell);
        CollideWithCellEntity(newCell);
        newCell.GameEntity = this;
        if (callSync)
            CallSync();
    }

    #endregion

    #region Private Methods

    private void EnterGates()
    {
        LevelManager.score++;
        Die();
    }

    #endregion
    
    #region MonoBehaviour Callbacks

    private void Awake()
    {
        CollisionInteractions = new()
        {
            {typeof(Enemy), e => DamageEntity(e, 1)},
            {typeof(Asteroid), e => DamageEntity(e, 1)},
            {typeof(Cargo), e => DamageEntity(e, 1)},
            {typeof(Gates), e => EnterGates()}
        };
    }

    #endregion
}