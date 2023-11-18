using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class PlayerList : MonoBehaviourPunCallbacks
{
    #region Private Fields

    private readonly List<PlayerItem> _listItems = new ();
    private int _lastPlayerCount = 1;
    private Room _room;
    private bool _localPlayerCalled;

    #endregion

    #region Serialized Fields

    [SerializeField] private ReadyButton readyButton;

    #endregion
    
    #region Static Fields

    private static readonly string ItemPrefabPath = "Prefabs/PlayerElement";

    #endregion

    #region Public Methods

    public void CheckPlayersReady()
    {
        foreach (var item in _listItems)
        {
            if (!item.Ready)
                return;
        }
        PhotonNetwork.LoadLevel("Game");
    }

    #endregion
    
    #region Private Methods

    private void LoadLocalPlayer(Player newPlayer)
    {
        _localPlayerCalled = true;
        if (!PhotonNetwork.InRoom)
        {
            Debug.LogError("not in room");
            // LoadLocalPlayer(newPlayer);
            return;
        }
        var itemObject = PhotonNetwork.Instantiate(
            ItemPrefabPath,
            transform.position,
            Quaternion.identity);
        var playerItem = itemObject.GetComponent<PlayerItem>();
        playerItem.ConnectToPlayer(newPlayer);
        readyButton.PlayerItem = playerItem;
        _listItems.Add(playerItem);
    }
    
    private void CheckPlayers()
    {
        foreach (var playerItem in FindObjectsOfType<PlayerItem>())
        {
            if (_listItems.Contains(playerItem))
            {
                continue;
            }
            _lastPlayerCount++;
            _listItems.Add(playerItem);
            playerItem.ConnectToList(this);
            var itemObject = playerItem.gameObject;
            itemObject.
                transform.SetParent(transform, worldPositionStays: false);
            itemObject.transform.localScale = Vector3.one;
        }
    }

    #endregion
    
    #region MonoBehaviour Callbacks
    
    private void Start()
    {
        _room = PhotonNetwork.CurrentRoom;
        CheckPlayers();
    }

    private void Update()
    {
        if (!_localPlayerCalled && PhotonNetwork.InRoom)
            LoadLocalPlayer(PhotonNetwork.LocalPlayer);
        if (_room.PlayerCount > _lastPlayerCount)
            CheckPlayers();
    }

    #endregion
}