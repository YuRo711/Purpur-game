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

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        if (!PhotonNetwork.IsMasterClient)
            Destroy(gameObject);
    }

    #endregion
}