using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour, IShipController
{
    [field: SerializeField] public float BasicChargeAmount { get; private set; }
    [field: SerializeField] public PanelButton[] Buttons { get; private set; }
    [field: SerializeField] public PlayerShip PlayerShip { get; set; }
    [field: SerializeField] public bool ButtonBreakingEnabled { get; private set; }
    [field: SerializeField] public bool IsChargedByTime { get; private set; }
    [field: SerializeField] public bool IsChargedByOtherPlayers { get; private set; }
    [field: SerializeField] public int ReceivedCharge { get; private set; } = 0;
    [field: SerializeField] public bool IsTestingModeEnabled { get; private set; }

    private void Start()
    {
        Buttons = GetComponentsInChildren<PanelButton>();
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
        PlayerShip.Shoot(direction);
    }

    public void Teleport(TurnDirections direction)
    {
        PlayerShip.Teleport(direction);
    }

    public void SpendCharge()
    {
        ReceivedCharge = Mathf.Max(0, ReceivedCharge - 1);
    }

    public void ReceiveCharge()
    {
        ReceivedCharge++;
    }
}
