namespace RManniBucau.Logging.RollingFile;

/// <summary>
/// The configuration of the rolling file provider.
/// </summary>
public class RollingFileOptions
{
    /// <summary>
    /// Directory to store logs and archives in. Note that directory MUST exist.
    /// </summary>
    public string Directory { get; set; } = ".";

    /// <summary>
    /// log (archive without extension) filename pattern, it can use {date} and {counter} templates.
    /// </summary>
    public string Filename { get; set; } = "log-{date}-{counter}.log";

    /// <summary>
    /// Are older than last files archives in gzip format.
    /// </summary>
    public bool Archive { get; set; } = true;

    /// <summary>
    /// How many archive days to keep is enabled.
    /// </summary>
    public int MaxDays { get; set; } = 7;

    /// <summary>
    /// How many chars to keep in memory before forcing a flush.
    /// This is indicative, the first time a message is read and lead to a buffer over this value will trigger a flush.
    /// </summary>
    public int BufferSize { get; set; } = 8_192;

    /// <summary>
    /// Max size of a single uncompressed file (in UTF-8 chars).
    /// </summary>
    public int MaxFileSize { get; set; } = 1024 * 1024 * 50;

    /// <summary>
    /// How long to await before forcing a flush if no message comes in.
    /// </summary>
    public int ForcedFlushTimeoutMillis { get; set; } = 30_000;

    /// <summary>
    /// For very advanced cases, a custom time provider - which can be optimized compared to default DateTime.Now.
    /// </summary>
    public Func<DateTime> TimeProvider { get; set; } = () => DateTime.Now;
}
