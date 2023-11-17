using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PanelButton : MonoBehaviour
{
    [field: SerializeField] public PanelButtonType ButtonType { get; private set; }
    [field: SerializeField] public bool IsFunctional { get; private set; }
    [field: SerializeField] public bool IsCharging { get; private set; }
    [field: SerializeField] public float ChargeAmount { get; private set; }
    [field: SerializeField] public float CurrentCharge { get; private set; }

    public event Action<PanelButton> OnBeginCharging;

    public bool IsFullyCharged
        => CurrentCharge >= 1;

    private void FixedUpdate()
    {
        UpdateCharge();
        UpdateAutoActivation();
    }

    [ContextMenu("Activate")]
    public virtual void Activate()
    {
        if (!IsFullyCharged || !IsFunctional)
            return;

        PerformShipAction();

        CurrentCharge = 0;

        if(ButtonType.BreaksAfterActivation)
            IsFunctional = false;
    }

    public void Repair()
    {
        IsFunctional = true;
    }

    public void StopCharging()
    {
        IsCharging = false;
    }

    private void UpdateCharge()
    {
        if (!IsFunctional)
            CurrentCharge = 0;

        else if(IsCharging)
            CurrentCharge = Mathf.Min(1, CurrentCharge + ChargeAmount);

        else if(!IsFullyCharged)
            CurrentCharge = Mathf.Max(0, CurrentCharge - ChargeAmount);
    }

    private void UpdateAutoActivation()
    {
        if(ButtonType.AutoActivatesWhenCharged && IsFullyCharged)
        {
            Activate();
        }
    }

    protected abstract void PerformShipAction();

    [ContextMenu("Start charging")]
    private void StartCharging()
    {
        IsCharging = true;
        OnBeginCharging?.Invoke(this);
    }
}
