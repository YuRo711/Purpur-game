public interface IPlayerControllable
{
    void MoveInDirection(TurnDirections direction);
    void TurnTo(TurnDirections direction);
    void Shoot(TurnDirections direction);
}