using SpaceGame.Core.Abstractions;

namespace SpaceGame.Core.Domain;

public class GameItem : IGameItem
{
    private readonly Dictionary<string, object> _properties = new();

    public object GetProperty(string key)
    {
        if (_properties.TryGetValue(key, out var value))
        {
            return value;
        }
        throw new KeyNotFoundException($"Свойство '{key}' не найдено в игровом объекте.");
    }

    public void SetProperty(string key, object value)
    {
        _properties[key] = value;
    }
}