using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Game.Control_Panel
{
    public class ButtonDeck : MonoBehaviourPunCallbacks
    {
        [field: SerializeField] public PanelButtonType[] ButtonTypes { get; private set; }
        public Queue<PanelButton> RequestQueue { get; private set; } = new Queue<PanelButton>();

        private Deck<int> indexDeck;

        void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var digits = Enumerable.Range(0, ButtonTypes.Length);
                indexDeck = new Deck<int>(digits);
            }
        }

        public void RequestNextButton(PanelButton button)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var ind = TakeNextButtonIndex();
                if (ind == null)
                    return;
                button.ButtonType = ButtonTypes[ind.Value];
            }
            else
            {
                RequestQueue.Enqueue(button);
                photonView.RPC("RequestButtonFromMasterClient", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber);
            }
        }

        [PunRPC]
        public void RequestButtonFromMasterClient(int requestingClientActorNumber)
        {
            //Debug.Log("[2/3] Master responding");
            var ind = TakeNextButtonIndex();
            if (ind == null)
                return;
            photonView.RPC("ReceiveButton", PhotonNetwork.CurrentRoom.GetPlayer(requestingClientActorNumber), ind.Value);
        }

        [PunRPC]
        public void ReceiveButton(int buttonIndex)
        {
            var button = RequestQueue.Dequeue();
            button.ButtonType = ButtonTypes[buttonIndex];
        }

        private int? TakeNextButtonIndex()
        {
            if (indexDeck == null)
                return null;

            return indexDeck.TakeNext();
        }
    }


}