import { HttpErrorResponse, HttpInterceptorFn, HttpResponse } from '@angular/common/http';
import { ErrorHandler, Injectable } from '@angular/core';
import {
  BrowserClient,
  defaultStackParser,
  getTraceData,
  logger,
  makeFetchTransport,
  Scope,
  setHttpStatus,
  startInactiveSpan,
  withScope,
} from '@sentry/browser';
import { defer, finalize, tap } from 'rxjs';
import manifest from '../../../extension/manifest.json';
import { environment } from '../../environments/environment';

declare const SENTRY_DSN: string;
declare const SENTRY_RELEASE: string;

let scope: Scope | undefined;

export function initializeSentry(): void {
  const dsn = typeof SENTRY_DSN === 'string' ? SENTRY_DSN : '';
  if (!dsn || scope) {
    return;
  }

  const client = new BrowserClient({
    dsn,
    environment: environment.production ? 'production' : 'development',
    release: typeof SENTRY_RELEASE === 'string' && SENTRY_RELEASE
      ? SENTRY_RELEASE
      : `sf-lab-extension@${manifest.version}`,
    integrations: [],
    stackParser: defaultStackParser,
    transport: makeFetchTransport,
    tracesSampleRate: environment.production ? 0.1 : 1.0,
    traceLifecycle: 'static',
    dataCollection: {
      userInfo: false,
      cookies: false,
      httpHeaders: false,
      httpBodies: [],
      urlQueryParams: false,
    },
  });

  scope = new Scope();
  scope.setClient(client);
  scope.setTag('component', 'extension');
  client.init();
}

export function captureExtensionError(error: unknown, operation: string): void {
  if (scope) {
    const errorScope = scope.clone();
    errorScope.setTag('operation', operation);
    errorScope.captureException(error);
    logger.error('Extension operation failed', { operation }, { scope: errorScope });
  }
}

@Injectable()
export class ExtensionErrorHandler extends ErrorHandler {
  override handleError(error: unknown): void {
    captureExtensionError(error, 'angular');
    super.handleError(error);
  }
}

export const sentryHttpInterceptor: HttpInterceptorFn = (request, next) => {
  const url = new URL(request.url, environment.apiUrl);
  if (!scope || url.origin !== new URL(environment.apiUrl).origin) {
    return next(request);
  }

  const extensionScope = scope;
  return defer(() => {
    const requestScope = extensionScope.clone();
    requestScope.setPropagationContext(new Scope().getPropagationContext());
    const span = withScope(requestScope, () => startInactiveSpan({
      name: `${request.method} ${url.pathname}`,
      op: 'http.client',
      parentSpan: null,
      attributes: {
        'http.request.method': request.method,
        'server.address': url.hostname,
        'url.full': `${url.origin}${url.pathname}`,
      },
    }));
    const { traceId, spanId } = span.spanContext();
    requestScope.setContext('trace', { trace_id: traceId, span_id: spanId });
    const traceData = withScope(requestScope, () => getTraceData({ span }));
    const headers = Object.fromEntries(Object.entries(traceData).filter((entry): entry is [string, string] =>
      typeof entry[1] === 'string'));

    return next(request.clone({ setHeaders: headers })).pipe(
      tap({
        next: event => {
          if (event instanceof HttpResponse) {
            setHttpStatus(span, event.status);
          }
        },
        error: (error: HttpErrorResponse) => {
          setHttpStatus(span, error.status);
          requestScope.captureException(new Error(`API request failed: ${request.method} ${url.pathname} (${error.status})`));
          logger.error('API request failed', {
            method: request.method,
            path: url.pathname,
            status: error.status,
          }, { scope: requestScope });
        },
      }),
      finalize(() => withScope(requestScope, () => span.end())),
    );
  });
};
