using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PlayerList : MonoBehaviourPunCallbacks
{
    #region Private Fields

    private readonly List<PlayerItem> _listItems = new ();

    #endregion

    #region Serialized Fields

    [SerializeField] private ReadyButton readyButton;

    #endregion
    
    #region Static Fields

    private static readonly string ItemPrefabPath = "Prefabs/PlayerElement";

    #endregion
    
    #region Private Methods

    /*private void LoadLocalPlayer(Player newPlayer)
    {
        var itemObject = PhotonNetwork.Instantiate(
            ItemPrefabPath,
            transform.position,
            Quaternion.identity);
        var playerItem = itemObject.GetComponent<PlayerItem>();
        playerItem.ConnectToPlayer(newPlayer);
        readyButton.PlayerItem = playerItem;
        _listItems.Add(playerItem);
    }*/
    
    #endregion

    #region MonoBehaviourPun Callbacks

    public override void OnLeftRoom()
    {
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        var itemObject = PhotonNetwork.Instantiate(
            ItemPrefabPath,
            transform.position,
            Quaternion.identity);
        var playerItem = itemObject.GetComponent<PlayerItem>();
        playerItem.ConnectToPlayer(newPlayer);
        _listItems.Add(playerItem);
    }

    #endregion

    #region MonoBehaviour Callbacks
    
    private void Start()
    {
        // LoadLocalPlayer(PhotonNetwork.LocalPlayer);
        foreach (var playerItem in FindObjectsOfType<PlayerItem>())
        {
            _listItems.Add(playerItem);
            playerItem.ConnectToList(this);
            var itemObject = playerItem.gameObject;
            itemObject.transform.parent = transform;
            itemObject.transform.localScale = Vector3.one;
        }
    }

    #endregion
}