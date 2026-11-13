using SpaceGame.Core.Abstractions;

namespace SpaceBattle.Core.Commands;

public class RotateCommand : ICommand
{
    private readonly IRotatingObject _rotatingObject;

    public RotateCommand(IRotatingObject rotatingObject)
    {
        _rotatingObject = rotatingObject ?? throw new ArgumentNullException(nameof(rotatingObject));
    }

    public void Execute()
    {
        _rotatingObject.Angle += _rotatingObject.AngularVelocity;
    }
}