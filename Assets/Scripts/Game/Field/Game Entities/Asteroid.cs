public class Asteroid : GameEntity
{
    #region MonoBehaviour Callbacks
    
    private void Awake()
    {
        health = 1;
        CollisionInteractions = new()
        {
            {typeof(PlayerShip), e => DamageEntity(e, 1, 1)}
        };
    }

    #endregion
}