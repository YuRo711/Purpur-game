using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    #region Serializable Fields

    [SerializeField] private float cellSize;
    [SerializeField] private int height;
    [SerializeField] private int width;
    [SerializeField] private GameObject cellPrefab;

    #endregion
    
    #region Properties

    public GridCell[,] Cells;

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        Cells = new GridCell[height, width];
        for (var x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                Cells[y, x] = new GridCell(x, y);
            }
    }

    #endregion
}
