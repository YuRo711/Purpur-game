using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Game.Control_Panel.UI
{
    public class KeyCodeText : MonoBehaviour
    {
        private PanelButton parentButton;
        private TextMeshProUGUI textMesh;

        void Start()
        {
            parentButton = GetComponentInParent<PanelButton>();
            textMesh = GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            textMesh.text = ((char)(parentButton.KeyCode)).ToString().ToUpper();
        }
    }
}