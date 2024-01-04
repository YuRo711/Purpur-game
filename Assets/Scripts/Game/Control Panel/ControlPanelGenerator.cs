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
    [SerializeField] private bool isTestingModeEnabled;
    [SerializeField] private GameObject testingPanel;

    #endregion
    
    
    #region Public Methods

    public void ConnectToPlayer(PlayerShip player)
    {
        controlPanel.PlayerShip = player;
    }
    
    #endregion
    
    #region MonoBehaviour Callbacks

    private void Awake()
    {
        GameObject panel;

        if (isTestingModeEnabled)
            panel = testingPanel;

        else
        {
            var index = PhotonNetwork.CurrentRoom.PlayerCount - 1;
            panel = playerPanels[index];
        }
        
        var newPanel = Instantiate(panel, transform);
        if (newPanel.TryGetComponent(out ControlPanel newControlPanel))
        {
            controlPanel = newControlPanel;
        }
    }

    #endregion
}