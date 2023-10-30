using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour, IPunObservable
{
    #region PrivateFields
    
    private bool _ready;
    private PlayerList _playerList;

    #endregion
    
    #region Serializable Fields

    [SerializeField] private Image readyImage;
    [SerializeField] private Sprite trueSprite;
    [SerializeField] private Sprite falseSprite;
    [SerializeField] private TMP_Text nicknameText;
    [SerializeField] private PhotonView photonView;

    #endregion

    #region Properties

    public string UserId { get; private set; }
    public string UserNickname { get; private set; }
    public bool IsMine { get; private set; }

    #endregion

    #region Public Methods

    public void ConnectToPlayer(Player player)
    {
        UserId = player.UserId;
        UserNickname = player.NickName;
        nicknameText.text = UserNickname;
    }

    public void ConnectToList(PlayerList playerList)
    {
        _playerList = playerList;
    }

    public void GetReady()
    {
        _ready = true;
        UpdateImage();
    }
    
    #endregion
    
    #region Private Methods

    [PunRPC]
    private void UpdateImage()
    {
        readyImage.sprite = _ready ? trueSprite : falseSprite;
    }

    #endregion
    
    #region IPunObservable Callbacks

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_ready);
        }
        else
        {
            _ready = (bool)stream.ReceiveNext();
            Debug.LogError("received " + _ready);
            photonView.RPC("UpdateImage", RpcTarget.AllBuffered);
        }
    }

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        IsMine = GetComponent<PhotonView>().IsMine;
        if (IsMine)
            ConnectToPlayer(PhotonNetwork.LocalPlayer);
        if (IsMine)
            Debug.LogError("I am " + UserNickname);
        else
            Debug.LogError("I am not " + UserNickname);
    }

    #endregion
}