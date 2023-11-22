using UnityEngine;

public class Signal : GameEntity
{
    #region Public Methods
    
    private void ChangePosition()
    {
        
    }

    #endregion
    
    #region MonoBehaviour Callbacks

    private void Awake()
    {
        isBackground = true;
    }

    #endregion
}