using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.Serialization;


public class GameLauncher : MonoBehaviourPunCallbacks
{
    #region Serializable Fields

    [SerializeField] private TMP_InputField roomInput;
    [SerializeField] private TMP_InputField nicknameInput;
    [SerializeField] private GameObject loader;
    
    #endregion
    
    #region MonoBehaviour
    
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        Connect();
    }

    #endregion
    
    #region MonoBehaviourPun Callbacks 

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(string.Format("Joined room {0}", PhotonNetwork.CurrentRoom.Name));
        PhotonNetwork.LoadLevel("RoomLobby");
        // loader.SetActive(true);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(roomInput.text);
    }

    #endregion


    #region Public Methods

    public void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void JoinRoom()
    {
        PhotonNetwork.NickName = nicknameInput.text;
        PhotonNetwork.JoinRoom(roomInput.text);
    }

    #endregion

}