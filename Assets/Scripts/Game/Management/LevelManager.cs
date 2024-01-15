using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviourPunCallbacks
{
    #region Public Fields

    [SerializeField] private GameObject GameOverText;
    [SerializeField] private GameObject RestartInstructionsText;

    public int Score { get; private set; } = 0;
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
    }

    public void RequestIncreaseScore()
    {
        Score++;
        StartCoroutine(CheckScore());
    }

    [PunRPC]
    void UpdateScore(int newScore)
    {
        if (newScore <= Score)
            return;

        Score = newScore;
        var bestScore = PlayerPrefs.GetInt("highscore");
        if(Score > bestScore)
        {
            PlayerPrefs.SetInt("highscore", Score);
        }
    }

    #endregion

    #region Private Methods

    private void Update()
    {
        CheckForEscape();
        CheckForGameOver();
    }

    private IEnumerator CheckScore()
    {
        yield return new WaitForSeconds(1);
        photonView.RPC("UpdateScore", RpcTarget.All, Score);
        soundManager.PlayAudioClip("deliver");
    }

    private void CheckForEscape()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
            return;

        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Menu");
    }

    private void CheckForGameOver()
    {
        if (!IsGameOver)
            return;

        GameOverText.SetActive(true);

        if (!PhotonNetwork.IsMasterClient)
            return;

        RestartInstructionsText.SetActive(true);

        if (Input.GetKeyDown(KeyCode.Space))
            photonView.RPC("RestartGame", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void RestartGame()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    #endregion
}