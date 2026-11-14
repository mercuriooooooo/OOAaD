using System.Collections.Concurrent;
using SpaceGame.Core.Abstractions;

namespace SpaceGame.Core.Ioc;
public interface IScope
{
    IDictionary<string, Func<object[], object>> Dependencies { get; }
    object Resolve(string key, object[] args);
}

public sealed class ActionCommand : ICommand
{
    private readonly Action _action;

    public ActionCommand(Action action) => _action = action ?? throw new ArgumentNullException(nameof(action));

    public void Execute() => _action();
}

internal sealed class Scope : IScope
{
    private readonly IScope? _parent;

    public IDictionary<string, Func<object[], object>> Dependencies { get; } =
        new ConcurrentDictionary<string, Func<object[], object>>();

    public Scope(IScope? parent = null)
    {
        _parent = parent;
    }

    public object Resolve(string key, object[] args)
    {
        if (Dependencies.TryGetValue(key, out var strategy))
        {
            return strategy(args);
        }

        if (_parent != null)
        {
            return _parent.Resolve(key, args);
        }

        throw new InvalidOperationException($"Зависимость '{key}' не зарегистрирована в текущем Scope.");
    }
}