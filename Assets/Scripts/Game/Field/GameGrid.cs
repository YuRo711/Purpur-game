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
    [SerializeField] private GridCell cellPrefab;
    [SerializeField] private RectTransform cellParent;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private int currentId;
    
    [SerializeField] public int enemiesCount;
    [SerializeField] public int cargoCount;
    [SerializeField] public int gatesCount;
    [SerializeField] public int asteroidsCount;

    #endregion

    #region Static Fields
    
    private static readonly string PlayerPrefabPath = "Prefabs/GameEntities/PlayerShip";
    private static readonly string EnemyPrefabPath = "Prefabs/GameEntities/EnemyShip";
    private static readonly string GatesPrefabPath = "Prefabs/GameEntities/Gates";
    private static readonly string CargoPrefabPath = "Prefabs/GameEntities/Cargo";
    private static readonly string AsteroidPrefabPath = "Prefabs/GameEntities/Asteroid";
    private static readonly string SignalPrefabPath = "Prefabs/GameEntities/Signal";
    private static readonly string[] Spawnable = { EnemyPrefabPath, CargoPrefabPath, AsteroidPrefabPath };
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

    public Tuple<int, int> GetRandomPosition()
    {
        var x = _random.Next(0, width - 1);
        var y = _random.Next(0, height - 1);
        var attempts = 0;
        while (Cells[y, x].GameEntity is not null && !Cells[y, x].GameEntity.isBackground)
        {
            if (attempts > width * height)
                return null;
            x = _random.Next(0, width - 1);
            y = _random.Next(0, height - 1);
            attempts++;
        }
        return Tuple.Create(x, y);
    }

    public void SpawnRandomSpawnable(int x, int y)
    {
        var index = _random.Next(0, Spawnable.Length - 1);
        SpawnEntityInCell(Spawnable[index], currentId, x, y);
        currentId++;
    }

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
        SpawnEntityType(PlayerPrefabPath, 1);
        SpawnEntityType(GatesPrefabPath, gatesCount);
        SpawnEntityType(CargoPrefabPath, cargoCount);
        SpawnEntityType(EnemyPrefabPath, enemiesCount);
        SpawnEntityType(AsteroidPrefabPath, asteroidsCount);
        SpawnEntityType(SignalPrefabPath, 1);
    }

    private void SpawnEntityType(string prefabPath, int count)
    {
        for (var i = currentId; i < currentId + count; i++)
            SpawnEntityOnRandom(prefabPath, i);
        currentId += count;
    }

    private void SpawnEntityOnRandom(string prefabPath, int id)
    {
        var position = GetRandomPosition();
        if (position is null)
            return;
        var x = position.Item1;
        var y = position.Item2;
        SpawnEntityInCell(prefabPath, id, x, y);
    }

    private void SpawnEntityInCell(string prefabPath, int id, int x, int y)
    {
        var entityObject = PhotonNetwork.Instantiate(
            prefabPath,
            transform.position,
            Quaternion.identity);
        var gameEntity = entityObject.GetComponent<GameEntity>();
        levelManager.AddEntity(gameEntity);
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
