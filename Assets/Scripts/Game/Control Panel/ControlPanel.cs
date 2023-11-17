using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    [field: SerializeField] public PanelButton[] Buttons { get; private set; }

    private void Start()
    {
        Buttons = GetComponentsInChildren<PanelButton>();
        foreach (var button in Buttons)
            button.OnStartCharging += ButtonSelected;
    }

    private void ButtonSelected(PanelButton button)
    {
        foreach (var b in Buttons)
        {
            if (b != button)
                b.StopCharging();
        }
    }
}
