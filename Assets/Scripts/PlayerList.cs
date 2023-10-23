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

    private List<(GameObject, string)> _listItems = new ();

    #endregion
    
    #region Serializable Fields

    [SerializeField] private GameObject itemPrefab;

    #endregion

    #region Private Mathodds

    private void Awake()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            AddPlayer(player);
        }
    }

    private void AddPlayer(Player newPlayer)
    {
        var newItem = Instantiate(itemPrefab, transform);
        _listItems.Add((newItem, newPlayer.UserId));
        newItem.GetComponentInChildren<TMP_Text>().text = newPlayer.NickName;
    }

    #endregion
    
    #region MonoBehaviourPunCallbacks
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
       AddPlayer(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        var item = _listItems
            .Single(item => item.Item2 == otherPlayer.UserId);
        _listItems.Remove(item);
        Destroy(item.Item1);
    }

    #endregion
}