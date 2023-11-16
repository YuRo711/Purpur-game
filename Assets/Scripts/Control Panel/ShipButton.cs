using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShipButton : MonoBehaviour
{
    public bool IsFunctioning { get; private set; }
    public float CurrentCharge { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool FullyCharged
        => CurrentCharge == 1;

    public ShipButton()
    {
        IsFunctioning = true;
        CurrentCharge = 0;
    }

    public virtual void Activate()
    {
        if (!IsFunctioning)
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
