using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForwardButton : ShipButton
{
    protected override void PerformShipAction()
    {
        Debug.Log("Moved forward!");
    }
}