using UnityEngine;

public class MockShip : IPlayerControllable
{
    public void MoveInDirection(TurnDirections direction)
    {
        Debug.Log($"Moved {direction}");
    }

    public void Shoot(TurnDirections direction)
    {
        Debug.Log($"Shot {direction}");
    }

    public void TurnTo(TurnDirections direction)
    {
        Debug.Log($"Turned {direction}");
    }
}