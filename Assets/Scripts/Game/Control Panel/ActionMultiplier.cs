using System.Collections;
using UnityEngine;

public class ActionMultiplier
{
    [field: SerializeField] public int Multiplier { get; private set; } = 1;

    public void IncrementMultiplier()
    {
        Multiplier++;
    }

    public void ResetMultiplier()
    {
        Multiplier = 1;
    }
}