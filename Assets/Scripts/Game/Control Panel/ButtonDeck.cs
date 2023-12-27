using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Control_Panel
{
    public class ButtonDeck : MonoBehaviour
    {
        [field: SerializeField] public PanelButtonType[] ButtonTypes { get; private set; }

        private Deck<PanelButtonType> buttonDeck;

        public void Start()
        {
            buttonDeck = new Deck<PanelButtonType>(ButtonTypes);
        }

        public PanelButtonType TakeNext()
        {
            return buttonDeck.TakeNext();
        }
    }
}