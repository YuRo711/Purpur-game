using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "MoveForwardButton", menuName = "PanelButton/MoveForward")]
public class MoveForwardButton : PanelButtonType
{
    public override void PerformAction(IShipController shipController)
    {
        shipController.Move(TurnDirections.Forward);
    }
}