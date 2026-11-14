using SpaceGame.Core.Domain;
namespace SpaceGame.Core.Abstractions;
public interface ICommand
{
    void Execute();
}
public interface IGameItem
{
    object GetProperty(string key);
    void SetProperty(string key, object value);
}

public interface IMovingObject
{
    Vector Location { get; set; }
    Vector Velocity { get; }
}

public interface IRotatingObject
{
    Angle Angle { get; set; }
    Angle AngularVelocity { get; }
}

public interface IShootingObject
{
    Vector Location { get; }
    Vector Velocity { get; }
    Angle Angle { get; }
    int Ammo { get; set; }
}