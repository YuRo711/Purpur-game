using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForwardButton : PanelButton
{
    protected override void PerformShipAction()
    {
        Debug.Log("Moved forward!");
    }
}