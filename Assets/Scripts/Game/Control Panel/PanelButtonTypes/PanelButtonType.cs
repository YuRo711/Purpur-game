using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class PanelButtonType : ScriptableObject
{
    [field: SerializeField] public bool BreaksAfterTrigger { get; private set; } = true;
    [field: SerializeField] public bool AutoTriggers { get; private set; } = false;

    [field: SerializeField] public Sprite BaseSprite { get; private set; }
    [field: SerializeField] public Sprite FullyChargedSprite { get; private set; }
    [field: SerializeField] public Sprite BrokenSprite { get; private set; }

    public abstract void PerformAction();
}