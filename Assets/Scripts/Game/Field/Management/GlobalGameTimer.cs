using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class GlobalGameTimer : MonoBehaviourPunCallbacks
{
    #region Serializable Fields

    [SerializeField] private float moveDuration = 4;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] public Signal signal;

    #endregion

    #region Properties

    public float TimerValue { get; private set; }

    #endregion
    
    #region Private Methods

    private IEnumerator Wait(float seconds)
    {
        while (TimerValue < seconds)
        {
            yield return new WaitForSeconds(0.01f);
            TimerValue += 0.01f;
        }
        ResetTimer();
    }

    private void ResetTimer()
    {
        TimerValue = 0;
        enemyManager.TakeActions();
        signal.UpdateSignal();
        StartCoroutine(Wait(moveDuration));
    }

    #endregion
    
    #region MonoBehaviour Callbacks

    private void Awake()
    {
        if (!PhotonNetwork.IsMasterClient)
            Destroy(gameObject);
        ResetTimer();
    }

    #endregion
}