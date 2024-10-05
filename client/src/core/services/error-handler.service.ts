import { ErrorHandler, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerService implements ErrorHandler {
  handleError(error: any): void {
    if (error.error && error.error.cause && error.error.cause.code === 'DEPTH_ZERO_SELF_SIGNED_CERT') {
      console.log('SSL complaining over', error.url);
      return;
    }

    if (error.error && error.error.cause) {
      console.error('An unexpected error occurred:', error.message, '\nCode:', error.error.cause.code);
      return;
    }

    console.error('An unexpected error occurred:', error);
  }
}
