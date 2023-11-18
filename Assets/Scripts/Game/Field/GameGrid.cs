using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class GameGrid : MonoBehaviourPunCallbacks
{
    #region Serializable Fields

    [SerializeField] private float cellSize;
    [SerializeField] public int height;
    [SerializeField] public int width;
    [SerializeField] public int enemiesCount;
    [SerializeField] private GridCell cellPrefab;
    [SerializeField] private RectTransform cellParent;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private ControlPanelGenerator controlPanelGenerator;

    #endregion

    #region Static Fields
    
    private static readonly string PlayerPrefabPath = "Prefabs/PlayerShip";
    private static readonly string EnemyPrefabPath = "Prefabs/EnemyShip";

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
        if (PhotonNetwork.IsMasterClient)
            GenerateEntities();
    }
    
    private void CreateCell (int x, int y)
    {
        GridCell cell = Cells[y, x] = Instantiate(cellPrefab, cellParent);
        cell.X = x;
        cell.Y = y;
    }

    private void GenerateEntities()
    {
        for (var i = 0; i < enemiesCount; i++)
            SpawnEntity(EnemyPrefabPath, i);
        SpawnEntity(PlayerPrefabPath, enemiesCount);
    }

    private void SpawnEntity(string prefabPath, int id)
    {
        Debug.Log("spawning " + prefabPath);
        var random = new System.Random();
        var x = random.Next(0, width - 1);
        var y = random.Next(0, height - 1);
        while (Cells[y, x].GameEntity is not null)
        {
            x = random.Next(0, width - 1);
            y = random.Next(0, height - 1);
        }
        var entityObject = PhotonNetwork.Instantiate(
            prefabPath,
            transform.position,
            Quaternion.identity);
        var gameEntity = entityObject.GetComponent<GameEntity>();
        gameEntity.SetStartParameters(id);
    }
    
    #endregion

    #region MonoBehaviour Callbacks

    private void Start()
    {
        Generate();
    }
    
    #endregion
}
