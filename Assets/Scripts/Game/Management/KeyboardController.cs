using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Management
{
    public class KeyboardController : MonoBehaviour
    {
        [field: SerializeField] public ControlPanelGenerator PanelGenerator { get; private set; }

        private static readonly KeyCode[] Keys = new KeyCode[]
        {
            KeyCode.Q,
            KeyCode.W,
            KeyCode.E,
            KeyCode.A,
            KeyCode.S,
            KeyCode.D,
        };

        private void Update()
        {
            if (!Input.anyKeyDown)
                return;

            for(var i = 0; i< Keys.Length; i++)
            {
                if (Input.GetKeyDown(Keys[i]))
                {
                    PanelGenerator.ControlPanel.Buttons[i].Trigger();
                }
            }
        }
    }
}