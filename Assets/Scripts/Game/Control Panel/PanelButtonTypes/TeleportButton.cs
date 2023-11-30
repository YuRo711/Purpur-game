using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "TeleportButton", menuName = "PanelButton/Teleport")]
public class TeleportButton : PanelButtonType
{
    public override void PerformAction(IShipController shipController)
    {
        shipController.Teleport(ActionDirection);
    }
}