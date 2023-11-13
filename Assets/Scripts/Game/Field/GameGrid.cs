using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameGrid : MonoBehaviour
{
    #region Serializable Fields

    [SerializeField] private float cellSize;
    [SerializeField] private int height;
    [SerializeField] private int width;
    [SerializeField] private GridCell cellPrefab;
    [SerializeField] private Transform cellParent;

    #endregion
    
    #region Properties

    public GridCell[,] Cells;

    #endregion

    #region Private Methods

    
    private void Generate()
    {
        Cells = new GridCell[height, width];
        for (var x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            CreateCell(x, y);
        }
    }
    
    private void CreateCell (int x, int y)
    {
        GridCell cell = Cells[y, x] = Instantiate(cellPrefab, cellParent.transform);
        cell.X = x;
        cell.Y = y;
    }



    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        Generate();
    }
    
    #endregion
}
