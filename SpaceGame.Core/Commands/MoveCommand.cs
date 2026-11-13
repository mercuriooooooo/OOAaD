using SpaceGame.Core.Abstractions;

namespace SpaceBattle.Core.Commands;

public class MoveCommand : ICommand
{
    private readonly IMovingObject _movingObject;

    public MoveCommand(IMovingObject movingObject)
    {
        _movingObject = movingObject ?? throw new ArgumentNullException(nameof(movingObject));
    }

    public void Execute()
    {
        _movingObject.Location += _movingObject.Velocity;
    }
}