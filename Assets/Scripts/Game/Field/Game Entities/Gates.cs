using System;

public class Gates : GameEntity
{
    #region MonoBehaviour Callbacks

    private void Awake()
    {
        isBackground = true;
    }

    #endregion
}