using UnityEngine;

public class GridCell : MonoBehaviour
{
    #region Properties

    [SerializeField] public GameEntity GameEntity;
    [SerializeField] public GameEntity BgEntity;
    [field: SerializeField] public int X { get; set; }
    [field: SerializeField] public int Y { get; set; }

    #endregion
}