using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "TurnRightButton", menuName = "PanelButton/TurnRight")]
public class TurnRightButton : PanelButtonType
{
    public override void PerformAction(IShipController shipController)
    {
        shipController.Turn(TurnDirections.Right);
    }
}