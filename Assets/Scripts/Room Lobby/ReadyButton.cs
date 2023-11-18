using System;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class ReadyButton : MonoBehaviour
{
    #region Properties

    public PlayerItem PlayerItem { get; set; }

    #endregion
    
    #region Public Methods

    public void GetReady()
    {
        PlayerItem.GetReady();
        PlayerItem.PlayerList.CheckPlayersReady();
    }

    #endregion
}