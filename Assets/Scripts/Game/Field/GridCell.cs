using UnityEngine;

public class GridCell : MonoBehaviour
{
    #region Properties

    public bool Taken { get; set; }
    public GameEntity GameEntity { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    #endregion
}