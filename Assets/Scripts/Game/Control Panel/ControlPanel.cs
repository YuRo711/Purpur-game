using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    [SerializeField] private float chargeAmount;

    [field: SerializeField] public PanelButton SelectedButton { get; private set; }

    void Update()
    {
        ChargeSelectedButton();
    }

    public void SelectButton(PanelButton button)
    {
        SelectedButton = button;
    }

    private void ChargeSelectedButton()
    {
        if(SelectedButton != null)
            SelectedButton.ChargeBy(chargeAmount);
    }
}
