using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New PanelButtonType", menuName = "Panel Button", order = 51)]
public class PanelButtonType : ScriptableObject
{
    [field: SerializeField] public bool BreaksAfterActivation { get; private set; } = true;
    [field: SerializeField] public bool AutoActivatesWhenCharged { get; private set; } = false;

    [field: SerializeField] public Sprite BaseSprite { get; private set; }
    [field: SerializeField] public Sprite ChargingSprite { get; private set; }
    [field: SerializeField] public Sprite FullyChargedSprite { get; private set; }
    [field: SerializeField] public Sprite BrokenSprite { get; private set; }
}