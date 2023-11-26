using UnityEngine;

public class GridCell : MonoBehaviour
{
    #region Properties

    public GameEntity GameEntity { get; set; }
    public GameEntity BgEntity { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    #endregion
}