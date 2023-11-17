using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour, IShipController
{
    [field: SerializeField] public PanelButton[] Buttons { get; private set; }
    [field: SerializeField] public PlayerShip PlayerShip { get; private set; }

    private void Start()
    {
        Buttons = GetComponentsInChildren<PanelButton>();
        foreach (var button in Buttons)
            button.OnStartCharging += ButtonSelected;
    }

    private void ButtonSelected(PanelButton button)
    {
        foreach (var b in Buttons)
        {
            if (b != button)
                b.StopCharging();
        }
    }

    public void Move(TurnDirections direction)
    {
        PlayerShip.MoveInDirection(direction);
    }

    public void Turn(TurnDirections direction)
    {
        PlayerShip.TurnTo(direction);
    }

    public void Shoot(TurnDirections direction)
    {
        throw new System.NotImplementedException();
    }
}
