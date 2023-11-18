using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour, IPunObservable
{
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
    
    public Player Player { get; private set; }

    public PlayerList PlayerList { get; private set; }
    
    public bool Ready { get; private set; }

    #endregion

    #region Public Methods

    public void ConnectToPlayer(Player player)
    {
        Player = player;
        UserId = player.UserId;
        UserNickname = player.NickName;
        nicknameText.text = UserNickname;
    }

    public void ConnectToList(PlayerList playerList)
    {
        PlayerList = playerList;
        transform.SetParent(playerList.transform, worldPositionStays: false);
    }

    public void GetReady()
    {
        Ready = true;
        UpdateImage();
    }
    
    #endregion
    
    #region Private Methods

    [PunRPC]
    private void UpdateImage()
    {
        readyImage.sprite = Ready ? trueSprite : falseSprite;
    }
    
    #endregion
    
    #region IPunObservable Callbacks

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Ready);
            stream.SendNext(UserNickname);
        }
        else
        {
            Ready = (bool)stream.ReceiveNext();
            UserNickname = (string)stream.ReceiveNext();
            nicknameText.text = UserNickname;
            photonView.RPC("UpdateImage", RpcTarget.AllBuffered);
        }
    }

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        ConnectToList(FindObjectOfType<PlayerList>());
        transform.localScale = Vector3.one;
        IsMine = GetComponent<PhotonView>().IsMine;
        if (IsMine)
            ConnectToPlayer(PhotonNetwork.LocalPlayer);
    }

    #endregion
}