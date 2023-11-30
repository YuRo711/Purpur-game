using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "RepairButton", menuName = "PanelButton/Repair")]
public class RepairButton : PanelButtonType
{
    public override void PerformAction(IShipController shipController)
    {
        shipController.RepairAll();
    }
}