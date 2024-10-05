import { ApplicationConfig, ErrorHandler, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { ErrorHandlerService } from '../core/services/error-handler.service';
import { authInterceptor } from '../core/services/auth.iterceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideHttpClient(
      withInterceptors([authInterceptor]),
      withFetch()
    ),
    provideRouter(routes),
    provideClientHydration(),
    {
      provide: ErrorHandler,
      useClass: ErrorHandlerService
    }
  ]
};
