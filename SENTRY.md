Sentry uses three projects in the `ernest-koguc` organization:

| Application | Sentry project |
| --- | --- |
| Blazor frontend | `sf-simulator` |
| ASP.NET Core API | `sf-simulator-api` |
| Angular extension | `sf-lab-extension` |

Select multiple projects in Sentry's Issues and Traces views to inspect them together. Extension API requests propagate `sentry-trace` and `baggage` headers so the extension and API spans belong to the same trace.

The API captures unhandled request errors and `ILogger` logs. Configure its runtime environment:

```sh
export SENTRY_DSN='https://a9d47a69c2b674cce1792a07079967ff@o4509968167403520.ingest.de.sentry.io/4512448498696272'.env.example
export SENTRY_ENVIRONMENT=development
dotnet run --project SFSimulator.Api
```

The launch profile supplies the development DSN automatically. The IIS deployment workflow generates `appsettings.Production.json` with the API DSN, production environment, deployed commit SHA as the release, and 10% trace sampling. Runtime environment variables `SENTRY_DSN`, `SENTRY_ENVIRONMENT`, and `SENTRY_RELEASE` can override these values. Automatic request-body attachments on error events and default user identity collection are disabled. Trace sampling defaults to 100% in Development and 10% elsewhere; `Sentry__TracesSampleRate` overrides it.

The API emits one structured `Information` log per completed request, including failures, through `SFSimulator.Api.RequestLoggingMiddleware`. Search Sentry Logs for `HTTP` or filter by that logger category. Fields are `RequestMethod`, `RequestUrl`, `StatusCode`, `DurationMs`, `RequestBody`, and `RequestBodyStatus`. These logs are independent of trace sampling.

Sentry stores the structured message parameters under `sentry.message.parameter.*`. For queries and columns in Sentry's API or CLI, use these field names:

| Value | Sentry field |
| --- | --- |
| HTTP method | `sentry.message.parameter.RequestMethod` |
| URL | `sentry.message.parameter.RequestUrl` |
| Status code | `tags[sentry.message.parameter.StatusCode,number]` |
| Duration in milliseconds | `tags[sentry.message.parameter.DurationMs,number]` |
| JSON body | `sentry.message.parameter.RequestBody` |
| Body capture status | `sentry.message.parameter.RequestBodyStatus` |

Status and duration are numeric attributes, so they can be filtered and aggregated without parsing the log message.

JSON request bodies up to 32 KiB are included, with credential fields redacted recursively. Sensitive query-string values are also redacted. The endpoint still receives the original request body. Bodies over the limit, invalid JSON, compressed bodies, and non-JSON uploads are omitted with a reason in `RequestBodyStatus`. Configure `RequestLogging:BodyLimitBytes` in appsettings or `RequestLogging__BodyLimitBytes` in the environment; `0` disables body capture while keeping request metadata, and the maximum is 1 MiB.

Release API builds upload debug symbols and sources when `SENTRY_AUTH_TOKEN` is present, using the same CI secret already used by the Blazor frontend. The token must have access to the new API project. Local builds without that token continue to work.

The extension runs inside the game's page. Its isolated Sentry client captures Angular errors, failed game-response processing, and failed API requests without installing global error or console handlers. Session Replay is disabled because it would record the host game's page. Logs contain operation names and API status codes, without game-response payloads. Trace sampling is 100% in development builds and 10% in production builds.

In `SFSimulator.Extension`, copy `.env.example` to `.env.local`, then use `npm start`, `npm run watch`, or `npm run build`. The scripts read `SENTRY_DSN` and `SENTRY_RELEASE` from the environment or `.env.local` and embed only those values in the browser bundle. Direct `ng` commands do not load `.env.local`. Set `SENTRY_RELEASE` to the deployed commit SHA, or omit it to use `sf-lab-extension@<manifest version>`. The browser DSN is public; never put an authentication token in client code.

Production extension builds generate hidden source maps. With the authenticated `sentry` CLI, run these commands before packaging the extension. The upload script finds the standard `~/.local/bin/sentry` installation automatically; set `SENTRY_CLI_PATH` for another location.

```sh
npm run build
npm run sentry:sourcemaps
```

The upload injects debug IDs into the JavaScript files as well as uploading their maps. Package those same JavaScript files after uploading. Exclude `.map` files from the extension package.
