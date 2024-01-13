using Assets.Scripts.Game.Control_Panel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelButton : MonoBehaviour
{
    [field: SerializeField] public PanelButtonType ButtonType { get; set; }
    [field: SerializeField] public float CurrentCharge { get; private set; }
    [field: SerializeField] public KeyCode KeyCode { get; set; }

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode))
            Trigger();
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

        ControlPanel.PlayerShip.ChargeManager.RequestSendCharge();
    }

    private void UpdateType()
    {
        if (ButtonType != null || ControlPanel.PlayerShip == null || ControlPanel.PlayerShip.ButtonDeck.RequestQueue.Contains(this))
            return;

        ControlPanel.PlayerShip.ButtonDeck.RequestNextButton(this);
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
