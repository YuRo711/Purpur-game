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
    [SerializeField] private int currentId;
    [SerializeField] private Signal signal;

    [SerializeField] public bool IsGhostHuntEnabled;
    [SerializeField] public int startingEnemiesCount;
    [SerializeField] public int startingCargoCount;
    [SerializeField] public int startingGatesCount;
    [SerializeField] public int startingAsteroidsCount;

    [SerializeField] private SoundManager soundManager;

    [field: SerializeField] public LevelManager LevelManager { get; private set; }

    #endregion

    #region Static Fields

    private static readonly Dictionary<Type, string> PrefabPaths = new()
    {
        { typeof(PlayerShip), "Prefabs/GameEntities/PlayerShip" },
        { typeof(Enemy), "Prefabs/GameEntities/EnemyShip" },
        { typeof(Gates), "Prefabs/GameEntities/Gates" },
        { typeof(Cargo), "Prefabs/GameEntities/Cargo" },
        { typeof(Asteroid), "Prefabs/GameEntities/Asteroid" },
        { typeof(Signal), "Prefabs/GameEntities/Signal" },
    };

    private static readonly Random _random = new();

    private static Deck<Type> SpawnDeck = new Deck<Type>(new[]
    {
        typeof(Gates),
        typeof(Cargo),
        typeof(Asteroid),
        typeof(Enemy),
    });

    #endregion

    #region Private Fields

    private readonly Dictionary<Type, int> ObjectCounts = new()
    {
        {typeof(Cargo), 0 },
        {typeof(Gates), 0 },
        {typeof(Enemy), 0 },
        {typeof(Asteroid), 0 },
        {typeof(PlayerShip), 0 },
        {typeof(Signal), 0 },
    };

    #endregion

    #region Properties

    public GridCell[,] Cells;

    #endregion

    #region Public Methods
    
    public void SpawnRandomAtSignalPosition()
    {
        if (signal == null)
        {
            SpawnEntityType(typeof(Signal), 1);
            return;
        }

        var oldX = signal.X;
        var oldY = signal.Y;
        if(signal.TryChangePosition())
            SpawnRandomSpawnable(oldX, oldY);
    }

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
            x = _random.Next(0, width);
            y = _random.Next(0, height);
            attempts++;
        }
        return Tuple.Create(x, y);
    }

    public void SpawnRandomSpawnable(int x, int y)
    {
        var objectToSpawn = PickObjectToSpawn();
        SpawnEntityInCell(objectToSpawn, currentId, x, y, true);
        currentId++;
    }

    public void OnEntityCannotSpawn()
    {
        LevelManager.FinishGame();
    }

    #endregion

    #region Private Methods
    
    private Type PickObjectToSpawn()
    {
        var asteroidsCount = ObjectCounts[typeof(Asteroid)];
        var enemiesCount = ObjectCounts[typeof(Enemy)];
        var cargoCount = ObjectCounts[typeof(Cargo)];
        var gateCount = ObjectCounts[typeof(Gates)];

        if (gateCount < 1)
            return typeof(Gates);

        if (cargoCount < 1)
            return typeof(Cargo);

        return SpawnDeck.TakeNext();
    }

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
        SpawnEntityType(typeof(PlayerShip), 1);
        SpawnEntityType(typeof(Gates), startingGatesCount);
        SpawnEntityType(typeof(Cargo), startingCargoCount);
        SpawnEntityType(typeof(Enemy), startingEnemiesCount);
        SpawnEntityType(typeof(Asteroid), startingAsteroidsCount);
        SpawnEntityType(typeof(Signal), 1);
    }

    private void SpawnEntityType(Type objectType, int count)
    {
        for (var i = currentId; i < currentId + count; i++)
            SpawnEntityOnRandom(objectType, i);
        currentId += count;
    }

    private void SpawnEntityOnRandom(Type objectType, int id)
    {
        var position = GetRandomPosition();
        if (position is null)
        {
            OnEntityCannotSpawn();
            return;
        }
        var x = position.Item1;
        var y = position.Item2;
        SpawnEntityInCell(objectType, id, x, y);
    }

    private void SpawnEntityInCell(Type objectType, int id, int x, int y, bool playAudio = false)
    {
        var entityObject = PhotonNetwork.Instantiate(
            PrefabPaths[objectType],
            transform.position,
            Quaternion.identity);
        var gameEntity = entityObject.GetComponent<GameEntity>();
        gameEntity.SetStartParameters(id, x, y);
        ObjectCounts[objectType]++;
        gameEntity.OnObjectDie += (obj, e) =>
        {
            ObjectCounts[e]--;
        };
        if (playAudio)
            soundManager.PlayAudioClip(gameEntity.spawnClip);

        if (objectType == typeof(Signal))
            signal = (Signal)gameEntity;

        gameEntity.CheckForGhost();
    }

    #endregion

    #region MonoBehaviour Callbacks

    private void Start()
    {
        Generate();
        LevelManager = FindObjectOfType<LevelManager>();
    }
    
    #endregion
}
