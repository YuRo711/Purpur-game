using Photon.Pun;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private PhotonView photonView;

    #endregion

    #region Public Methods

    public void PlayAudioClip(AudioClip clip)
    {
        if (clip is null)
            return;
        photonView.RPC("PlaySoundRPC", RpcTarget.AllBuffered, clip);
    }

    #endregion

    #region Private Methods

    [PunRPC]
    private void PlaySoundRPC(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    #endregion
}