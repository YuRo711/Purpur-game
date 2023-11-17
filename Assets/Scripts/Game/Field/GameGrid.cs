using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameGrid : MonoBehaviour
{
    #region Serializable Fields

    [SerializeField] private float cellSize;
    [SerializeField] public int height;
    [SerializeField] public int width;
    [SerializeField] public int enemiesCount;
    [SerializeField] private GridCell cellPrefab;
    [SerializeField] private RectTransform cellParent;

    #endregion
    
    #region Properties

    public GridCell[,] Cells;

    #endregion
    

    #region Private Methods
    
    private void Generate()
    {
        cellParent.sizeDelta = new Vector2(width * cellSize, height * cellSize);
        Cells = new GridCell[height, width];
        for (var x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            CreateCell(x, y);
        }
    }
    
    private void CreateCell (int x, int y)
    {
        GridCell cell = Cells[y, x] = Instantiate(cellPrefab, cellParent);
        cell.X = x;
        cell.Y = y;
    }

    // private void GenerateEntities()
    // {
    //     var random = new System.Random();
    //     for (var i = 0; i < enemiesCount; i++)
    //     {
    //         var x = random.Next(0, width - 1);
    //         var y = random.Next(0, height - 1);
    //         
    //     }
    //     
    //     var playerX = random.Next(0, width - 1);
    //     var playerY = random.Next(0, height - 1);
    // }
    
    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        Generate();
    }
    
    #endregion
}
