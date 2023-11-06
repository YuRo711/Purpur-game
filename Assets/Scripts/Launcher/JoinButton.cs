using Photon.Pun;
using UnityEngine;

public class JoinButton : MonoBehaviour
{
    #region Private Serializable Fields
    
    [SerializeField] private GameLauncher gameLauncher;
    
    #endregion
    
    
    #region MonoBehaviour CallBacks

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    #endregion

    #region Public Methods

    public void OnClick()
    {
        gameLauncher.JoinRoom();
    }
    
    #endregion
}
