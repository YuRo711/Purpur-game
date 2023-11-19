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
    [SerializeField] private int currentIndex;
    [SerializeField] private ControlPanel controlPanel;

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
        // КОСТЫЛЬ
        var index = PhotonNetwork.IsMasterClient ? 0 : 1;
        var panel = playerPanels[index];
        var newPanel = Instantiate(panel, transform);
        if (newPanel.TryGetComponent(out ControlPanel newControlPanel))
        {
            controlPanel = newControlPanel;
        }
    }

    #endregion
}