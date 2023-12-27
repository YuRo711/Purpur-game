using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class PanelButtonType : ScriptableObject
{
    [field: SerializeField] public TurnDirections ActionDirection { get; private set; }

    [field: SerializeField] public Sprite BaseSprite { get; private set; }

    public abstract void PerformAction(IShipController shipController);
}