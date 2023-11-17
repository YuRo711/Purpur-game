using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelButton : MonoBehaviour
{
    [field: SerializeField] public PanelButtonType ButtonType { get; private set; }
    [field: SerializeField] public bool IsFunctional { get; private set; } = true;
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

    [ContextMenu("Trigger")]
    public virtual void Trigger()
    {
        if (!IsFullyCharged || !IsFunctional)
            return;

        ButtonType.PerformAction();

        CurrentCharge = 0;

        if(ButtonType.BreaksAfterTrigger)
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
        if(ButtonType.AutoTriggers && IsFullyCharged)
        {
            Trigger();
        }
    }

    [ContextMenu("Start charging")]
    private void StartCharging()
    {
        IsCharging = true;
        OnBeginCharging?.Invoke(this);
    }
}
