using System.Buffers;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;

namespace SFSimulator.Api;

public sealed class RequestLoggingMiddleware(
    RequestDelegate next,
    ILogger<RequestLoggingMiddleware> logger,
    IOptions<RequestLoggingOptions> options)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (!logger.IsEnabled(LogLevel.Information))
        {
            await next(context);
            return;
        }

        var started = Stopwatch.GetTimestamp();
        var body = new LoggedBody(null, "empty");
        Exception? failure = null;

        try
        {
            body = await ReadBodyAsync(context);
            await next(context);
        }
        catch (Exception exception)
        {
            failure = exception;
            throw;
        }
        finally
        {
            var statusCode = context.Response.StatusCode;
            if (failure is not null && !context.Response.HasStarted)
            {
                statusCode = failure switch
                {
                    BadHttpRequestException badRequest => badRequest.StatusCode,
                    OperationCanceledException when context.RequestAborted.IsCancellationRequested => 499,
                    _ => StatusCodes.Status500InternalServerError,
                };
            }

            logger.LogInformation(
                "HTTP {RequestMethod} {RequestUrl} responded {StatusCode} in {DurationMs} ms. Request body ({RequestBodyStatus}): {RequestBody}",
                context.Request.Method,
                GetRequestUrl(context.Request),
                statusCode,
                Stopwatch.GetElapsedTime(started).TotalMilliseconds,
                body.Status,
                body.Value);
        }
    }

    private async Task<LoggedBody> ReadBodyAsync(HttpContext context)
    {
        var request = context.Request;
        if (request.ContentLength == 0 || context.Features.Get<IHttpRequestBodyDetectionFeature>()?.CanHaveBody == false)
        {
            return new(null, "empty");
        }

        var limit = options.Value.BodyLimitBytes;
        if (limit == 0)
        {
            return new(null, "disabled");
        }

        if (!request.HasJsonContentType() || request.Headers.ContentEncoding.Count > 0)
        {
            return new(null, "unsupported-content-type-or-encoding");
        }

        if (request.ContentLength > limit)
        {
            return new(null, "body-limit-exceeded");
        }

        request.EnableBuffering();
        var position = request.Body.Position;
        var buffer = ArrayPool<byte>.Shared.Rent(limit + 1);
        try
        {
            var length = await request.Body.ReadAtLeastAsync(buffer.AsMemory(0, limit + 1), limit + 1,
                throwOnEndOfStream: false, context.RequestAborted);
            if (length == 0)
            {
                return new(null, "empty");
            }

            if (length > limit)
            {
                return new(null, "body-limit-exceeded");
            }

            using var document = JsonDocument.Parse(buffer.AsMemory(0, length));
            using var output = new MemoryStream();
            using (var writer = new Utf8JsonWriter(output))
            {
                WriteRedactedJson(writer, document.RootElement);
            }

            return new(Encoding.UTF8.GetString(output.GetBuffer(), 0, (int)output.Length), "recorded");
        }
        catch (JsonException)
        {
            return new(null, "invalid-json");
        }
        catch (IOException)
        {
            return new(null, "unavailable");
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            return new(null, "unavailable");
        }
        finally
        {
            request.Body.Position = position;
            ArrayPool<byte>.Shared.Return(buffer, clearArray: true);
        }
    }

    private static string GetRequestUrl(HttpRequest request)
    {
        var query = QueryString.Create(request.Query.SelectMany(parameter => parameter.Value.Select(value =>
            new KeyValuePair<string, string?>(parameter.Key, IsSensitive(parameter.Key) ? "[REDACTED]" : value))));
        return UriHelper.BuildAbsolute(request.Scheme, request.Host, request.PathBase, request.Path, query);
    }

    private static void WriteRedactedJson(Utf8JsonWriter writer, JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                writer.WriteStartObject();
                foreach (var property in element.EnumerateObject())
                {
                    writer.WritePropertyName(property.Name);
                    if (IsSensitive(property.Name))
                    {
                        writer.WriteStringValue("[REDACTED]");
                    }
                    else
                    {
                        WriteRedactedJson(writer, property.Value);
                    }
                }
                writer.WriteEndObject();
                break;
            case JsonValueKind.Array:
                writer.WriteStartArray();
                foreach (var item in element.EnumerateArray())
                {
                    WriteRedactedJson(writer, item);
                }
                writer.WriteEndArray();
                break;
            default:
                element.WriteTo(writer);
                break;
        }
    }

    private static bool IsSensitive(string name)
    {
        var normalized = name.Replace("_", "").Replace("-", "");
        return normalized.Equals("apiKey", StringComparison.OrdinalIgnoreCase)
            || normalized.Equals("pwd", StringComparison.OrdinalIgnoreCase)
            || normalized.Contains("password", StringComparison.OrdinalIgnoreCase)
            || normalized.Contains("passwd", StringComparison.OrdinalIgnoreCase)
            || normalized.Contains("token", StringComparison.OrdinalIgnoreCase)
            || normalized.Contains("secret", StringComparison.OrdinalIgnoreCase)
            || normalized.Contains("authorization", StringComparison.OrdinalIgnoreCase)
            || normalized.Contains("credential", StringComparison.OrdinalIgnoreCase)
            || normalized.Contains("cookie", StringComparison.OrdinalIgnoreCase)
            || normalized.StartsWith("session", StringComparison.OrdinalIgnoreCase);
    }

    private readonly record struct LoggedBody(string? Value, string Status);
}
