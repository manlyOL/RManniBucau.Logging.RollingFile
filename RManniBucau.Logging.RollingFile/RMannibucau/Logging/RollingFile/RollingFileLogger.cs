using Microsoft.Extensions.Logging;

namespace RManniBucau.Logging.RollingFile;

internal class RollingFileLogger(
    string category,
    MessageQueue queue,
    IExternalScopeProvider? scopeProvider
) : ILogger
{
    public IDisposable BeginScope<TState>(TState state)
        where TState : notnull =>
        scopeProvider?.Push(state) ?? EmptyExternalScopeProvider.EmptyScope.Instance;

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    )
    {
        if (IsEnabled(logLevel))
        {
            queue.EnqueueMessage(logLevel, category, formatter(state, exception));
        }
    }
}
