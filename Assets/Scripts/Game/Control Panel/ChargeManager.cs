using System.Collections.Generic;
using UnityEngine;

public class ChargeManager
{
    private readonly Dictionary<ControlPanel, int> indexes;
    private readonly List<ControlPanel> panels;
    
    public ChargeManager()
    {
        indexes = new Dictionary<ControlPanel, int>();
        panels = new List<ControlPanel>();
    }

    public void AddPanel(ControlPanel panel)
    {
        indexes.Add(panel, panels.Count);
        panels.Add(panel);
        Debug.LogError($"Got {indexes.Count} indexes! And {panels.Count} panels, yeah");
    }

    public void ReceiveChargeFrom(ControlPanel panel)
    {
        var ind = (indexes[panel] + 1) % panels.Count;
        panels[ind].ReceiveCharge();
        Debug.LogError($"Giving charge to {ind}");
    }
}