using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PanelButton : MonoBehaviour
{
    [field: SerializeField] public bool IsFunctioning { get; private set; }
    [field: SerializeField] public float ChargeAmount { get; private set; }
    [field: SerializeField] public float CurrentCharge { get; private set; }

    private bool isCharging;
    public bool IsCharging
    {
        get => isCharging;
        set
        {
            isCharging = value;
        }
    }

    public bool IsFullyCharged
        => CurrentCharge >= 1;

    void Start()
    {
        IsFunctioning = true;
        CurrentCharge = 0;
    }

    private void FixedUpdate()
    {
        UpdateCharge();
    }

    [ContextMenu("Activate")]
    public virtual void Activate()
    {
        if (!IsFullyCharged || !IsFunctioning)
            return;

        PerformShipAction();

        CurrentCharge = 0;
        IsFunctioning = false;
    }

    public void Repair()
    {
        IsFunctioning = true;
    }

    public void StopCharging()
    {
        IsCharging = false;
    }

    private void UpdateCharge()
    {
        if (!IsFunctioning)
            CurrentCharge = 0;

        else if(IsCharging)
            CurrentCharge = Mathf.Min(1, CurrentCharge + ChargeAmount);

        else if(!IsFullyCharged)
            CurrentCharge = Mathf.Max(0, CurrentCharge - ChargeAmount);
    }

    protected abstract void PerformShipAction();

    [ContextMenu("Start charging")]
    private void StartCharging()
    {
        IsCharging = true;
    }
}
