public sealed class LogFileSettings
{
    public const string SectionName = "LogFileSettings";

    public required string FilePath { get; init; }

    public int MaxReadRetries { get; init; } = 3;

    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(30);

    public bool IsValid() => !string.IsNullOrWhiteSpace(FilePath);
}