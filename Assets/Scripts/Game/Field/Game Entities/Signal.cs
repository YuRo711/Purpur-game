using System;
using System.Threading;
using Photon.Pun;
using UnityEngine;

public class Signal : GameEntity
{
    [SerializeField] private GameTimer timer;

    #region Public Methods
    
    public bool TryChangePosition()
    {
        var pos = levelGrid.GetRandomPosition();
        if (pos is null)
        {
            //Die();
            levelGrid.OnEntityCannotSpawn();
            return false;
        }
        MoveTo(pos.Item1, pos.Item2);
        return true;
    }

    #endregion

    #region Private Methods

    private void Update()
    {
        if(timer.TimeIsUp)
        {
            transform.Rotate(0, 0, 45);
            timer.Restart();
        }
    }

    private void Start()
    {
        timer.Restart();
    }

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        health = 1;
    }

    #endregion
}