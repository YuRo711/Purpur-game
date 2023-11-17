using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "TurnLeftButton", menuName = "PanelButton/TurnLeft")]
public class TurnLeftButton : PanelButtonType
{
    public override void PerformAction(IShipController shipController)
    {
        shipController.Turn(TurnDirections.Left);
    }
}