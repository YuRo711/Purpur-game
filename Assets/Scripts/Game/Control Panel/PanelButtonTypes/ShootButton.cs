using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ShootButton", menuName = "PanelButton/Shoot")]
public class ShootButton : PanelButtonType
{
    public override void PerformAction(IShipController shipController)
    {
        shipController.Shoot(ActionDirection);
    }
}