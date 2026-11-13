using SpaceGame.Core.Abstractions;

namespace SpaceGame.Core.Ioc;

public static class IoC
{
    private static readonly Scope RootScope = new( null);
    
    private static readonly ThreadLocal<IScope?> CurrentScope = new(() => null);

    private static IScope ActiveScope => CurrentScope.Value ?? RootScope;
    static IoC()
    {
        InitializeCoreDependencies();
    }

    public static T Resolve<T>(string key, params object[] args)
    {
        var result = ActiveScope.Resolve(key, args);
        if (result is T typedResult)
        {
            return typedResult;
        }

        throw new InvalidCastException(
            $"Зависимость '{key}' вернула тип '{result?.GetType().FullName ?? "null"}', ожидался '{typeof(T).FullName}'.");
    }

    private static void InitializeCoreDependencies()
    {

        RootScope.Dependencies["IoC.Register"] = args =>
        {
            var key = (string)args[0];
            var strategy = (Func<object[], object>)args[1];
            return new ActionCommand(() => ActiveScope.Dependencies[key] = strategy);
        };

        RootScope.Dependencies["IoC.Register.Singleton"] = args =>
        {
            var key = (string)args[0];
            var target = args[1];

            return new ActionCommand(() =>
            {
                if (target is Func<object[], object> factory)
                {
                    var lazyInstance = new Lazy<object>(() => factory(Array.Empty<object>()), LazyThreadSafetyMode.ExecutionAndPublication);
                    ActiveScope.Dependencies[key] = _ => lazyInstance.Value;
                }
                else
                {
                    ActiveScope.Dependencies[key] = _ => target;
                }
            });
        };

        RootScope.Dependencies["Scopes.New"] = args =>
        {
            var parent = args.Length > 0 && args[0] is IScope customParent
                ? customParent
                : ActiveScope;
            return new Scope(parent);
        };

        RootScope.Dependencies["Scopes.Current"] = _ => ActiveScope;
        RootScope.Dependencies["Scopes.Current.Set"] = args =>
        {
            var scope = args.Length > 0 ? args[0] as IScope : null;
            return new ActionCommand(() => CurrentScope.Value = scope);
        };

        RootScope.Dependencies["Scopes.Root"] = _ => RootScope;
    }
}