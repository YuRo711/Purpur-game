using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private PhotonView photonView;

    private const string ClipsDirectory = "Sounds/";

    #endregion

    #region Public Methods

    public void PlayAudioClip(string clipLink)
    {
        photonView.RPC("PlaySoundRPC", RpcTarget.AllBuffered, clipLink);
    }

    #endregion

    #region Private Methods

    [PunRPC]
    private void PlaySoundRPC(string clipLink)
    {
        var path = ClipsDirectory + clipLink;
        var clip = Resources.Load<AudioClip>(path);
        Debug.Log("trying to play " + path);
        if (clip is null)
            return;
        Debug.Log("playing " + path);
        audioSource.clip = clip;
        audioSource.Play();
    }

    #endregion
}