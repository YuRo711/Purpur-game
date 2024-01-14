using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class GlobalGameTimer : MonoBehaviourPunCallbacks
{
    #region Serializable Fields

    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private GameGrid grid;
    [field: SerializeField] public LevelManager LevelManager { get; private set; }
    [field: SerializeField] public GameTimer EnemyTimer { get; private set; }
    [field: SerializeField] public GameTimer SpawnTimer { get; private set; }

    #endregion

    #region Private Methods

    private void Update()
    {
        if (LevelManager.IsGameOver)
            return;

        if(EnemyTimer.TimeIsUp)
        {
            enemyManager.TakeActions();
            EnemyTimer.Restart();
        }

        if (SpawnTimer.TimeIsUp)
        {
            grid.SpawnRandomAtSignalPosition();
            SpawnTimer.Restart();
        }
    }

    #endregion
    
    #region MonoBehaviour Callbacks

    private void Awake()
    {
        if (!PhotonNetwork.IsMasterClient)
            Destroy(gameObject);
        EnemyTimer.Restart();
        SpawnTimer.Restart();
    }

    #endregion
}