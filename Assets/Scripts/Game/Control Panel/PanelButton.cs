using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PanelButton : MonoBehaviour
{
    [field: SerializeField] public bool IsFunctioning { get; private set; }
    [field: SerializeField] public float CurrentCharge { get; private set; }

    void Start()
    {
        IsFunctioning = true;
        CurrentCharge = 0;
    }

    void Update()
    {
        CurrentCharge = Mathf.Min(1, CurrentCharge);
    }

    public bool IsFullyCharged
        => CurrentCharge >= 1;

    [ContextMenu("Activate")]
    public virtual void Activate()
    {
        if (!IsFunctioning || !IsFullyCharged)
            return;

        PerformShipAction();

        CurrentCharge = 0;
        IsFunctioning = false;
    }

    public void ChargeBy(float amount)
    {
        if (!IsFunctioning)
            return;

        CurrentCharge = Mathf.Min(1, CurrentCharge + amount);
    }

    public void Repair()
    {
        IsFunctioning = true;
    }

    protected abstract void PerformShipAction();
}
