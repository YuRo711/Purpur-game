using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChargeManager : MonoBehaviourPunCallbacks
{
    private Dictionary<int, int> actorNumberToIndex;
    private List<int> indexToActorNumber;
    private ControlPanel localPanel;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            actorNumberToIndex = new Dictionary<int, int>();
            indexToActorNumber = new List<int>();
        }
    }

    public void RequestRegisterPlayer(ControlPanel panel)
    {
        localPanel = panel;

        if (PhotonNetwork.IsMasterClient)
        {
            RegisterPlayer(PhotonNetwork.LocalPlayer.ActorNumber);
        }

        else
        {
            photonView.RPC("RegisterPlayer", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }

    [PunRPC]
    void RegisterPlayer(int playerActorNumber)
    {
        var ind = indexToActorNumber.Count;
        actorNumberToIndex.Add(playerActorNumber, ind);
        indexToActorNumber.Add(playerActorNumber);

        Debug.Log($"Player {playerActorNumber} registered at index {actorNumberToIndex[playerActorNumber]}");
        Debug.Log($"Current player list: {string.Join(', ', indexToActorNumber)}");
    }

    public void ReceiveChargeFrom(ControlPanel panel)
    {
        return;
    }
}