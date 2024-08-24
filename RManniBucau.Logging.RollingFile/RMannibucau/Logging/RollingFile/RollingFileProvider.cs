using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace RManniBucau.Logging.RollingFile;

/// <summary>
/// The rolling file provider.
/// </summary>
[ProviderAlias("RollingFile")]
public class RollingFileProvider : ILoggerProvider, ISupportExternalScope
{
    private readonly ConcurrentDictionary<string, RollingFileLogger> _loggers = new();
    private readonly MessageQueue _queue;

    private readonly RollingFileOptions _options;
    private IExternalScopeProvider? _scopeProvider = EmptyExternalScopeProvider.Instance;

    /// <summary>
    /// Creates a new rolling file provider.
    /// </summary>
    /// <param name="options">customizations of the log output</param>
    public RollingFileProvider(RollingFileOptions? options = null)
    {
        _options = options ?? new();
        _queue = new(_options);
    }

#if DEBUG
    internal MessageQueue Queue => _queue;
#endif

    /// <inheritdoc/>
    public ILogger CreateLogger(string name)
    {
        return _loggers.GetOrAdd(name, new RollingFileLogger(name, _queue, _scopeProvider));
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _queue.Dispose();
    }

    /// <inheritdoc/>
    public void SetScopeProvider(IExternalScopeProvider scopeProvider)
    {
        _scopeProvider = scopeProvider;
    }
}
