public interface IShipController
{
    public void Move(TurnDirections direction);
    public void Turn(TurnDirections direction);
    public void Shoot(TurnDirections direction);
    public void Teleport(TurnDirections direction);
}