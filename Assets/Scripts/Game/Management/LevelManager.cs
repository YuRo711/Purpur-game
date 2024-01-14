using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Public Fields

    public int score = 0;
    public GameGrid levelGrid;
    public GlobalGameTimer timer;
    public EnemyManager enemyManager;
    public PlayerShip player;
    public ControlPanelGenerator controlPanelGenerator;
    public SoundManager soundManager;

    [field: SerializeField] public bool IsGameOver { get; private set; } = false;

    #endregion

    #region Public Methods

    public void AddEntity(GameEntity gameEntity)
    {
        gameEntity.LevelManager = this;
        if (gameEntity is PlayerShip playerShip)
            player = playerShip;
        if (gameEntity is Enemy enemy)
            enemyManager.enemies.Add(enemy);
    }

    public void FinishGame()
    {
        IsGameOver = true;
        Debug.Log("GAME OVER");
    }

    #endregion

    #region Private Methods

    private void Update()
    {
        if(IsGameOver && Input.GetKeyDown(KeyCode.Space))
            RestartGame();
    }

    private void RestartGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion
}