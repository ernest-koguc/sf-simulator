import { provideHttpClient, withInterceptors, withXhr } from '@angular/common/http';
import { ApplicationConfig, ErrorHandler, provideZonelessChangeDetection } from '@angular/core';
import { ExtensionErrorHandler, sentryHttpInterceptor } from './monitoring/sentry';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZonelessChangeDetection(),
    provideHttpClient(withXhr(), withInterceptors([sentryHttpInterceptor])),
    { provide: ErrorHandler, useClass: ExtensionErrorHandler },
  ]
};
