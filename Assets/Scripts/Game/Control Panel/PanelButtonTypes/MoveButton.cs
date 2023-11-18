using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "MoveButton", menuName = "PanelButton/Move")]
public class MoveButton : PanelButtonType
{
    public override void PerformAction(IShipController shipController)
    {
        shipController.Move(ActionDirection);
    }
}