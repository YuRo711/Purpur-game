public class Bullet : AutoEntity
{
    #region Public Methods

    public override void TakeAction()
    {
        MoveInDirection(TurnDirections.Forward);
    }

    public override void MoveTo(int destX, int destY)
    {
        if (CheckForBorder(destX, destY))
            Die();
        x = destX;
        y = destY;
        var newCell = LevelGrid.Cells[x, y];
        if (newCell.GameEntity is GameEntity gameEntity)
        {
            gameEntity.TakeDamage(1);
            Die();
        }
    }

    #endregion
    
}