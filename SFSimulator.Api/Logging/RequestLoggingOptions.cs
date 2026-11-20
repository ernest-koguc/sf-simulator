namespace SFSimulator.Api;

public sealed class RequestLoggingOptions
{
    public int BodyLimitBytes { get; set; } = 32 * 1024;
}
