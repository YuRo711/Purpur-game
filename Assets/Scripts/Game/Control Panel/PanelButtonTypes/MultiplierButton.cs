using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "MultiplierButton", menuName = "PanelButton/Multiplier")]
public class MultiplierButoon : PanelButtonType
{
    public override void PerformAction(IShipController shipController)
    {
        shipController.IncrementMultiplier();
    }
}