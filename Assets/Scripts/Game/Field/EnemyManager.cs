using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class EnemyManager : MonoBehaviourPunCallbacks
{
    #region Public Fields

    public List<Enemy> enemies = new List<Enemy>();

    #endregion

    #region Public Methods

    public void TakeActions()
    {
        foreach (var enemy in enemies)
        {
            enemy.TakeAction();
        }
    }

    public void LookForPlayer()
    {
        foreach (var enemy in enemies)
        {
            enemy.LookForPlayer();
        }
    }

    #endregion
    
    #region MonoBehaviour Callbacks

    // private void Awake()
    // {
    //     if (!PhotonNetwork.IsMasterClient)
    //         Destroy(gameObject);
    // }

    #endregion
}