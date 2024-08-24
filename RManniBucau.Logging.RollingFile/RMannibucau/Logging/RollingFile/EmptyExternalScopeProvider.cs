using Microsoft.Extensions.Logging;

namespace RManniBucau.Logging.RollingFile;

internal sealed class EmptyExternalScopeProvider : IExternalScopeProvider
{
    internal static EmptyExternalScopeProvider Instance = new();

    private EmptyExternalScopeProvider() { }

    void IExternalScopeProvider.ForEachScope<TState>(
        Action<object?, TState> callback,
        TState state
    ) { }

    IDisposable IExternalScopeProvider.Push(object? state)
    {
        return EmptyScope.Instance;
    }

    internal sealed class EmptyScope : IDisposable
    {
        public static EmptyScope Instance { get; } = new EmptyScope();

        private EmptyScope() { }

        public void Dispose() { }
    }
}
