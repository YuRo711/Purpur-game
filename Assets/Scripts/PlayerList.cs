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

    private void Awake()
    {
        LoadLocalPlayer(PhotonNetwork.LocalPlayer);
    }

    private void LoadLocalPlayer(Player newPlayer)
    {
        // var newItem = Instantiate(
        //     Resources.Load<GameObject>(_itemPrefabPath),
        //     transform);
        var itemObject = PhotonNetwork.Instantiate(
            ItemPrefabPath,
            transform.position,
            Quaternion.identity);
        itemObject.transform.parent = transform;
        itemObject.transform.localScale = Vector3.one;
        var playerItem = itemObject.GetComponent<PlayerItem>();
        playerItem.ConnectToPlayer(newPlayer);
        playerItem.ConnectToList(this);
        readyButton.PlayerItem = playerItem;
        _listItems.Add(playerItem);
    }
    
    #endregion

    #region MonoBehaviourPun Callbacks

    public override void OnLeftRoom()
    {
    }

    #endregion
}