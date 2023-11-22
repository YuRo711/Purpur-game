using System;
using Photon.Pun;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Serializable Fields

    [SerializeField] public int score = 0;
    [SerializeField] public GameGrid levelGrid;
    [SerializeField] public GlobalGameTimer timer;
    [SerializeField] public EnemyManager enemyManager;
    [SerializeField] public PlayerShip player;
    [SerializeField] public ControlPanelGenerator controlPanelGenerator; 

    #endregion


    #region Public Methods

    public void AddEntity(GameEntity gameEntity)
    {
        gameEntity.LevelManager = this;
        if (gameEntity is PlayerShip playerShip)
            player = playerShip;
        if (gameEntity is Enemy enemy)
            enemyManager.enemies.Add(enemy);
        if (gameEntity is Signal signal)
            timer.signal = signal;
    }

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        if (!PhotonNetwork.IsMasterClient)
            Destroy(gameObject);
    }

    #endregion
}