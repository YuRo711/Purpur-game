using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ChargeManager : MonoBehaviourPunCallbacks
{
    private Dictionary<int, int> actorNumberToIndex = new Dictionary<int, int>();
    private List<int> indexToActorNumber = new List<int>();
    private ControlPanel localPanel;

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
    }

    public void RequestSendCharge()
    {
        photonView.RPC("SendCharge", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber);
    }

    [PunRPC]
    void SendCharge(int sendingActorNumber)
    {
        var ind = actorNumberToIndex[sendingActorNumber];
        var targetInd = (ind + 1) % indexToActorNumber.Count;
        var targetPlayerNumber = indexToActorNumber[targetInd];
        photonView.RPC("ReceiveCharge", PhotonNetwork.CurrentRoom.GetPlayer(targetPlayerNumber));

        //Debug.Log($"{ind} sent charge to {targetInd}");
    }

    [PunRPC]
    void ReceiveCharge()
    {
        localPanel.ReceiveCharge();

        Debug.Log($"Got charge!");
    }
}