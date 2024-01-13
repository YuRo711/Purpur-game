using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private PhotonView photonView;
    [SerializeField] private bool soundEnabled;

    private const string ClipsDirectory = "Sounds/";

    #endregion

    #region Public Methods

    public void PlayAudioClip(string clipLink)
    {
        if (!soundEnabled)
            return;

        photonView.RPC("PlaySoundRPC", RpcTarget.AllBuffered, clipLink);
    }

    #endregion

    #region Private Methods

    [PunRPC]
    private void PlaySoundRPC(string clipLink)
    {
        var path = ClipsDirectory + clipLink;
        var clip = Resources.Load<AudioClip>(path);
        if (clip is null)
            return;
        audioSource.clip = clip;
        audioSource.Play();
    }

    #endregion
}