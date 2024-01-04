using System;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class ControlPanelGenerator : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private List<GameObject> playerPanels;
    [SerializeField] private ControlPanel controlPanel;

    #endregion
    
    
    #region Public Methods

    public void ConnectToPlayer(PlayerShip player)
    {
        controlPanel.PlayerShip = player;
        player.ChargeManager.AddPanel(controlPanel);
    }
    
    #endregion
    
    #region MonoBehaviour Callbacks

    private void Awake()
    {
        var index = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        var panel = playerPanels[index];
        var newPanel = Instantiate(panel, transform);
        if (newPanel.TryGetComponent(out ControlPanel newControlPanel))
        {
            controlPanel = newControlPanel;
        }
    }

    #endregion
}