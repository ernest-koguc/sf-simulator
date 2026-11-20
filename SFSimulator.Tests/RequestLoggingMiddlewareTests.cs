using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFSimulator.Api;

namespace SFSimulator.Tests;

[TestClass]
public class RequestLoggingMiddlewareTests
{
    [TestMethod]
    [DataRow("application/json")]
    [DataRow("application/json; charset=utf-8")]
    [DataRow("application/vnd.sf-simulator+json")]
    public async Task Logs_request_fields_and_preserves_nonseekable_body(string contentType)
    {
        const string body = "{ \"Player\": { \"Level\": 100 }, \"Iterations\": 10 }";
        var context = CreateContext(body, contentType);
        context.Request.QueryString = new QueryString("?world=light");
        string? receivedBody = null;

        var logger = await InvokeAsync(context, async current =>
        {
            receivedBody = await new StreamReader(current.Request.Body).ReadToEndAsync();
            current.Response.StatusCode = 201;
        });

        Assert.AreEqual(body, receivedBody);
        Assert.AreEqual(1, logger.Entries.Count);
        var entry = logger.Entries.Single();
        Assert.AreEqual("POST", entry["RequestMethod"]);
        Assert.AreEqual("https://api.example/api/dungeon/simulate?world=light", entry["RequestUrl"]);
        Assert.AreEqual(201, entry["StatusCode"]);
        Assert.IsTrue((double)entry["DurationMs"]! >= 0);
        Assert.AreEqual("recorded", entry["RequestBodyStatus"]);
        using var json = JsonDocument.Parse((string)entry["RequestBody"]!);
        Assert.AreEqual(100, json.RootElement.GetProperty("Player").GetProperty("Level").GetInt32());
        Assert.AreEqual(10, json.RootElement.GetProperty("Iterations").GetInt32());
    }

    [TestMethod]
    public async Task Redacts_nested_credentials_and_query_values_without_changing_the_request()
    {
        const string body = """
            {"password":"private-password","Player":{"Level":100,"items":[{"API_KEY":"private-key","access_token":"private-token"}]},"sessionId":"private-session"}
            """;
        var context = CreateContext(body);
        context.Request.QueryString = new QueryString("?world=light&auth_token=private-query&world=shadow");

        var logger = await InvokeAsync(context, async current =>
        {
            Assert.AreEqual(body, await new StreamReader(current.Request.Body).ReadToEndAsync());
            Assert.AreEqual("private-query", current.Request.Query["auth_token"].ToString());
        });

        var entry = logger.Entries.Single();
        var loggedBody = (string)entry["RequestBody"]!;
        Assert.IsFalse(loggedBody.Contains("private-", StringComparison.Ordinal));
        using var json = JsonDocument.Parse(loggedBody);
        Assert.AreEqual("[REDACTED]", json.RootElement.GetProperty("password").GetString());
        Assert.AreEqual("[REDACTED]", json.RootElement.GetProperty("sessionId").GetString());
        var item = json.RootElement.GetProperty("Player").GetProperty("items")[0];
        Assert.AreEqual("[REDACTED]", item.GetProperty("API_KEY").GetString());
        Assert.AreEqual("[REDACTED]", item.GetProperty("access_token").GetString());
        var url = Uri.UnescapeDataString((string)entry["RequestUrl"]!);
        Assert.IsTrue(url.Contains("auth_token=[REDACTED]", StringComparison.Ordinal));
        Assert.IsFalse(url.Contains("private-query", StringComparison.Ordinal));
        Assert.IsTrue(url.Contains("world=light", StringComparison.Ordinal));
        Assert.IsTrue(url.Contains("world=shadow", StringComparison.Ordinal));
    }

    [TestMethod]
    [DataRow(200)]
    [DataRow(204)]
    [DataRow(400)]
    [DataRow(404)]
    public async Task Logs_empty_requests_and_returned_status_codes(int statusCode)
    {
        var context = CreateContext("");
        context.Request.Method = "GET";
        var logger = await InvokeAsync(context, current =>
        {
            current.Response.StatusCode = statusCode;
            return Task.CompletedTask;
        });

        var entry = logger.Entries.Single();
        Assert.AreEqual(statusCode, entry["StatusCode"]);
        Assert.IsNull(entry["RequestBody"]);
        Assert.AreEqual("empty", entry["RequestBodyStatus"]);
    }

    [TestMethod]
    public async Task Logs_unhandled_failure_as_500_and_rethrows_the_original_exception()
    {
        var context = CreateContext("{}");
        var logger = new RecordingLogger();
        var failure = new InvalidOperationException("request failed");
        var middleware = CreateMiddleware(_ => throw failure, logger);

        var thrown = await Assert.ThrowsExactlyAsync<InvalidOperationException>(() => middleware.InvokeAsync(context));

        Assert.AreSame(failure, thrown);
        Assert.AreEqual(500, logger.Entries.Single()["StatusCode"]);
    }

    [TestMethod]
    public async Task Preserves_bad_request_exception_status()
    {
        var context = CreateContext("{}");
        var logger = new RecordingLogger();
        var middleware = CreateMiddleware(_ => throw new BadHttpRequestException("too large", 413), logger);

        await Assert.ThrowsExactlyAsync<BadHttpRequestException>(() => middleware.InvokeAsync(context));

        Assert.AreEqual(413, logger.Entries.Single()["StatusCode"]);
    }

    [TestMethod]
    public async Task Logs_disconnected_requests_as_499_and_propagates_cancellation()
    {
        var context = CreateContext("");
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        context.RequestAborted = cancellation.Token;
        var logger = new RecordingLogger();
        var middleware = CreateMiddleware(_ => throw new OperationCanceledException(cancellation.Token), logger);

        await Assert.ThrowsExactlyAsync<OperationCanceledException>(() => middleware.InvokeAsync(context));

        Assert.AreEqual(499, logger.Entries.Single()["StatusCode"]);
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public async Task Omits_oversized_bodies_and_preserves_all_bytes_for_the_endpoint(bool knownLength)
    {
        var body = "{\"password\":\"private-value\",\"padding\":\"" + new string('a', 200) + "\"}";
        var context = CreateContext(body);
        if (!knownLength)
        {
            context.Request.ContentLength = null;
        }

        var logger = await InvokeAsync(context, async current =>
            Assert.AreEqual(body, await new StreamReader(current.Request.Body).ReadToEndAsync()), 32);

        var entry = logger.Entries.Single();
        Assert.IsNull(entry["RequestBody"]);
        Assert.AreEqual("body-limit-exceeded", entry["RequestBodyStatus"]);
    }

    [TestMethod]
    public async Task Captures_a_body_exactly_at_the_limit()
    {
        const string body = "{\"value\":123}";
        var logger = await InvokeAsync(CreateContext(body), _ => Task.CompletedTask, Encoding.UTF8.GetByteCount(body));

        Assert.AreEqual("recorded", logger.Entries.Single()["RequestBodyStatus"]);
    }

    [TestMethod]
    public async Task Omits_invalid_json_and_leaves_it_available_for_the_endpoint()
    {
        const string body = "{\"password\":\"private-value\"";
        var logger = await InvokeAsync(CreateContext(body), async current =>
        {
            Assert.AreEqual(body, await new StreamReader(current.Request.Body).ReadToEndAsync());
            current.Response.StatusCode = 400;
        });

        Assert.IsNull(logger.Entries.Single()["RequestBody"]);
        Assert.AreEqual("invalid-json", logger.Entries.Single()["RequestBodyStatus"]);
        Assert.AreEqual(400, logger.Entries.Single()["StatusCode"]);
    }

    [TestMethod]
    [DataRow("multipart/form-data; boundary=example")]
    [DataRow("application/octet-stream")]
    [DataRow("text/plain")]
    public async Task Omits_unsupported_bodies_without_consuming_them(string contentType)
    {
        const string body = "private-file-content";
        var context = CreateContext(body, contentType);
        var original = context.Request.Body;
        var logger = await InvokeAsync(context, async current =>
        {
            Assert.AreSame(original, current.Request.Body);
            Assert.AreEqual(body, await new StreamReader(current.Request.Body).ReadToEndAsync());
        });

        Assert.IsNull(logger.Entries.Single()["RequestBody"]);
        Assert.AreEqual("unsupported-content-type-or-encoding", logger.Entries.Single()["RequestBodyStatus"]);
    }

    [TestMethod]
    public async Task Body_logging_can_be_disabled_without_disabling_request_metadata()
    {
        var context = CreateContext("{\"value\":1}");
        var original = context.Request.Body;
        var logger = await InvokeAsync(context, current =>
        {
            Assert.AreSame(original, current.Request.Body);
            return Task.CompletedTask;
        }, 0);

        Assert.IsNull(logger.Entries.Single()["RequestBody"]);
        Assert.AreEqual("disabled", logger.Entries.Single()["RequestBodyStatus"]);
        Assert.AreEqual("POST", logger.Entries.Single()["RequestMethod"]);
    }

    [TestMethod]
    public async Task Disabled_information_logging_does_not_read_or_buffer_the_body()
    {
        var context = CreateContext("{\"value\":1}");
        var original = context.Request.Body;
        var logger = new RecordingLogger { Enabled = false };
        var called = false;
        var middleware = CreateMiddleware(current =>
        {
            called = true;
            Assert.AreSame(original, current.Request.Body);
            return Task.CompletedTask;
        }, logger);

        await middleware.InvokeAsync(context);

        Assert.IsTrue(called);
        Assert.AreEqual(0, logger.Entries.Count);
    }

    private static DefaultHttpContext CreateContext(string body, string contentType = "application/json")
    {
        var context = new DefaultHttpContext();
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("api.example");
        context.Request.PathBase = "/api";
        context.Request.Path = "/dungeon/simulate";
        context.Request.Method = "POST";
        context.Request.ContentType = contentType;
        var bytes = Encoding.UTF8.GetBytes(body);
        context.Request.ContentLength = bytes.Length;
        context.Request.Body = new NonSeekableReadStream(bytes);
        return context;
    }

    private static async Task<RecordingLogger> InvokeAsync(HttpContext context, RequestDelegate next, int limit = 32768)
    {
        var logger = new RecordingLogger();
        await CreateMiddleware(next, logger, limit).InvokeAsync(context);
        return logger;
    }

    private static RequestLoggingMiddleware CreateMiddleware(RequestDelegate next, RecordingLogger logger, int limit = 32768)
        => new(next, logger, Options.Create(new RequestLoggingOptions { BodyLimitBytes = limit }));

    private sealed class RecordingLogger : ILogger<RequestLoggingMiddleware>
    {
        public bool Enabled { get; init; } = true;
        public List<Dictionary<string, object?>> Entries { get; } = [];
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
        public bool IsEnabled(LogLevel logLevel) => Enabled;
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter)
            => Entries.Add(((IEnumerable<KeyValuePair<string, object?>>)state!).ToDictionary(pair => pair.Key, pair => pair.Value));
    }

    private sealed class NonSeekableReadStream(byte[] bytes) : Stream
    {
        private readonly MemoryStream inner = new(bytes);
        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
        public override int Read(byte[] buffer, int offset, int count) => inner.Read(buffer, offset, count);
        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
            => inner.ReadAsync(buffer, cancellationToken);
        public override void Flush() => throw new NotSupportedException();
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        protected override void Dispose(bool disposing)
        {
            if (disposing) inner.Dispose();
            base.Dispose(disposing);
        }
    }
}
