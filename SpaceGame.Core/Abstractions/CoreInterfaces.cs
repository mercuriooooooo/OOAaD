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
