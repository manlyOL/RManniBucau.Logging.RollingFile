# RManniBucau.Logging.RollingFile

Integrates with _Microsoft.Extensions.Logging_ to provide a file based logger provider with rotation (per date and approximative size) and archiving in gzip.

## Usage

```cs
var options = new RollingFileOptions
{
    Directory = _dir,
    //...
};
_factory = LoggerFactory.Create(builder =>
{
    builder.AddProvider(new RollingFileProvider(options));
});
```

## Configuration

The configuration is centralized in `RollingFileOptions`:

<!-- regenerated with "dotnet run --project RManniBucau.Logging.RollingFile.Build/" -->
<!-- Start Generated Content -->
Name | Summary | Default
| :----- | :----: | -----:
Archive | Are older than last files archives in gzip format. | `True`
BufferSize | How many chars to keep in memory before forcing a flush.             This is indicative, the first time a message is read and lead to a buffer over this value will trigger a flush. | `8192`
Directory | Directory to store logs and archives in. Note that directory MUST exist. | `.`
Filename | log (archive without extension) filename pattern, it can use {date} and {counter} templates. | `log-{date}-{counter}.log`
ForcedFlushTimeoutMillis | How long to await before forcing a flush if no message comes in. | `30000`
MaxDays | How many archive days to keep is enabled. | `7`
MaxFileSize | Max size of a single uncompressed file (in UTF-8 chars). | `52428800`
TimeProvider | For very advanced cases, a custom time provider - which can be optimized compared to default DateTime.Now. | `() => DateTime.Now`
<!-- End Generated Content -->
