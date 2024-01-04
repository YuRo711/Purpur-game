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

        CurrentCharge = 0;
        ButtonType = null;
    }

    private void UpdateType()
    {
        if (ButtonType != null)
            return;

        ButtonType = ControlPanel.PlayerShip.ButtonDeck.TakeNext();
    }

    private void UpdateCharge()
    {
        if (ControlPanel.IsChargedByTime)
        {
            CurrentCharge = Mathf.Min(1, CurrentCharge + ControlPanel.BasicChargeAmount);
            return;
        }

        if(ControlPanel.IsChargedByOtherPlayers && CurrentCharge == 0 && ControlPanel.ReceivedCharge > 0)
        {
            ControlPanel.SpendCharge();
            CurrentCharge = 1;
        }
    }

    private void PerformAction()
    {
        ButtonType.PerformAction(ControlPanel);
    }
}
