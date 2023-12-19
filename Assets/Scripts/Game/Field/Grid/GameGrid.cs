using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

enum GameObjects
{
    Player,
    Enemy,
    Cargo,
    Asteroid,
    Gate,
    Signal
}

public class GameGrid : MonoBehaviourPunCallbacks
{
    #region Serializable Fields

    [SerializeField] private float cellSize;
    [SerializeField] public int height;
    [SerializeField] public int width;
    [SerializeField] private GridCell cellPrefab;
    [SerializeField] private RectTransform cellParent;
    [SerializeField] private int currentId;
    
    [SerializeField] public int enemiesCount;
    [SerializeField] public int cargoCount;
    [SerializeField] public int gatesCount;
    [SerializeField] public int asteroidsCount;

    #endregion

    #region Static Fields

    private static readonly Dictionary<GameObjects, string> PrefabPaths = new()
    {
        { GameObjects.Player, "Prefabs/GameEntities/PlayerShip" },
        { GameObjects.Enemy, "Prefabs/GameEntities/EnemyShip" },
        { GameObjects.Gate, "Prefabs/GameEntities/Gates" },
        { GameObjects.Cargo, "Prefabs/GameEntities/Cargo" },
        { GameObjects.Asteroid, "Prefabs/GameEntities/Asteroid" },
        { GameObjects.Signal, "Prefabs/GameEntities/Signal" },
    };
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
        var x = _random.Next(0, width);
        var y = _random.Next(0, height);
        var attempts = 0;
        while (Cells[y, x].GameEntity is not null || Cells[y, x].BgEntity is not null)
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
        //TODO: Implement choice of object
        var objectToSpawn = GameObjects.Enemy;
        SpawnEntityInCell(PrefabPaths[objectToSpawn], currentId, x, y);
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
        SpawnEntityType(PrefabPaths[GameObjects.Player], 1);
        SpawnEntityType(PrefabPaths[GameObjects.Gate], gatesCount);
        SpawnEntityType(PrefabPaths[GameObjects.Cargo], cargoCount);
        SpawnEntityType(PrefabPaths[GameObjects.Enemy], enemiesCount);
        SpawnEntityType(PrefabPaths[GameObjects.Asteroid], asteroidsCount);
        SpawnEntityType(PrefabPaths[GameObjects.Signal], 1);
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
        gameEntity.SetStartParameters(id, x, y);
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
