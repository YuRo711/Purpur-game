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
    [SerializeField] private PlayerShip playerShip;
    [SerializeField] private int currentIndex;

    #endregion
    #region MonoBehaviour Callbacks

    private void Awake()
    {
        // КОСТЫЛЬ
        var index = PhotonNetwork.IsMasterClient ? 0 : 1;
        var panel = playerPanels[index];
        var newPanel = Instantiate(panel, transform);
        if (newPanel.TryGetComponent(out ControlPanel controlPanel))
        {
            controlPanel.PlayerShip = playerShip;
        }
    }

    #endregion
}