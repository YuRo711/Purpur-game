using System;
using Photon.Pun;
using Photon.Realtime;

public class Loader : MonoBehaviourPunCallbacks
{
    #region MonoBehaviour Callbacks

    private void Update()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
            PhotonNetwork.LoadLevel("RoomLobby");
    }

    #endregion
}