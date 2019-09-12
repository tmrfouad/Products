import { ErrorHandler, Injectable, Injector, NgZone } from '@angular/core';

@Injectable()
export class CustomeErrorHandler implements ErrorHandler {
  handleError(error): void {
    alert(error.error ? error.error.Message : error.message);
    console.error('Error log :', error);
    // throw error;
  }
}
