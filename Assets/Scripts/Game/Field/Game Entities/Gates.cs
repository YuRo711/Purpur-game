using System;

public class Gates : GameEntity
{
    #region MonoBehaviour Callbacks

    private void Awake()
    {
        isBackground = false;
        health = 1;
        CollisionInteractions = new()
        {
            {typeof(PlayerShip), e => DamageEntity(e, 1, 1)},
            {typeof(Enemy), e => DamageEntity(e, 1, 1)},
        };
    }

    #endregion
}