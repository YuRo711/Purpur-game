using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "TurnButton", menuName = "PanelButton/Turn")]
public class TurnButton : PanelButtonType
{
    public override void PerformAction(IShipController shipController)
    {
        shipController.Turn(ActionDirection);
    }
}