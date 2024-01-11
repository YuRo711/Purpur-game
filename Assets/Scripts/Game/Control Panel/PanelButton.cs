using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelButton : MonoBehaviour
{
    [field: SerializeField] public PanelButtonType ButtonType { get; private set; }
    [field: SerializeField] public float CurrentCharge { get; private set; }

    public ControlPanel ControlPanel { get; private set; }

    public bool IsFullyCharged
        => CurrentCharge >= 1;

    private void Start()
    {
        ControlPanel = GetComponentInParent<ControlPanel>();
    }

    private void FixedUpdate()
    {
        UpdateType();
        UpdateCharge();
    }

    public void HandleClick()
    {
        if (IsFullyCharged)
            Trigger();
    }

    [ContextMenu("Trigger")]
    public virtual void Trigger()
    {
        if (!IsFullyCharged)
            return;

        PerformAction();

        if (ControlPanel.IsTestingModeEnabled)
            return;

        CurrentCharge = 0;
        ButtonType = null;
    }

    private void UpdateType()
    {
        if (ButtonType != null || ControlPanel.PlayerShip == null)
            return;

        ButtonType = ControlPanel.PlayerShip.ButtonDeck.TakeNext();
    }

    private void UpdateCharge()
    {
        CurrentCharge = Mathf.Min(1, CurrentCharge + ControlPanel.BasicChargeAmount);
    }

    private void PerformAction()
    {
        ButtonType.PerformAction(ControlPanel);
        return;
    }
}
