using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class GameGrid : MonoBehaviourPunCallbacks
{
    #region Serializable Fields

    [SerializeField] private float cellSize;
    [SerializeField] public int height;
    [SerializeField] public int width;
    [SerializeField] public int enemiesCount;
    [SerializeField] public int cargoCount;
    [SerializeField] public int gatesCount;
    [SerializeField] private GridCell cellPrefab;
    [SerializeField] private RectTransform cellParent;
    [FormerlySerializedAs("levelManager2")] [SerializeField] private LevelManager levelManager;

    #endregion

    #region Static Fields
    
    private static readonly string PlayerPrefabPath = "Prefabs/GameEntities/PlayerShip";
    private static readonly string EnemyPrefabPath = "Prefabs/GameEntities/EnemyShip";
    private static readonly string GatesPrefabPath = "Prefabs/GameEntities/Gates";
    private static readonly string CargoPrefabPath = "Prefabs/GameEntities/Cargo";
    private static Random _random;

    #endregion
    
    #region Properties

    public GridCell[,] Cells;

    #endregion

    
    #region Public Methods
    
    public bool CheckForBorder(int newX, int newY)
    {
        return newX >= width || newX < 0 ||
               newY >= height || newY < 0;
    }

    #endregion

    #region Private Methods
    
    private void Generate()
    {
        if (levelManager is null)
            throw new Exception("Where is the level manager?");
            
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
        SpawnEntity(PlayerPrefabPath, 0);
        for (var i = 1; i <= gatesCount; i++)
            SpawnEntity(GatesPrefabPath, i);
        var id = gatesCount + 1;
        for (var i = id; i <= id + cargoCount; i++)
            SpawnEntity(CargoPrefabPath, i);
        id = gatesCount + cargoCount + 2;
        for (var i = id; i <= id + enemiesCount; i++)
            SpawnEntity(EnemyPrefabPath, i);
    }

    private void SpawnEntity(string prefabPath, int id)
    {
        Debug.Log("spawning " + prefabPath);
        var x = _random.Next(0, width - 1);
        var y = _random.Next(0, height - 1);
        while (Cells[y, x].GameEntity is not null)
        {
            x = _random.Next(0, width - 1);
            y = _random.Next(0, height - 1);
        }
        var entityObject = PhotonNetwork.Instantiate(
            prefabPath,
            transform.position,
            Quaternion.identity);
        var gameEntity = entityObject.GetComponent<GameEntity>();
        
        gameEntity.LevelManager = levelManager;
        if (gameEntity is PlayerShip playerShip)
            levelManager.player = playerShip;
        
        gameEntity.SetStartParameters(id);
        gameEntity.MoveTo(x, y);
    }
    
    #endregion

    #region MonoBehaviour Callbacks

    private void Start()
    {
        _random = new System.Random();
        Generate();
    }
    
    #endregion
}
