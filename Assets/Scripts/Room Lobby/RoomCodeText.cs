using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Room_Lobby
{
    public class RoomCodeText : MonoBehaviour
    {
        private TextMeshProUGUI text;

        void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
            text.text = PhotonNetwork.CurrentRoom.Name;
        }
    }
}