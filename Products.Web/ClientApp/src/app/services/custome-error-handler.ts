import { ErrorHandler, Injectable, Injector, NgZone } from '@angular/core';
import { MatDialog } from '@angular/material';
import { ErrorDialogComponent } from '../error-dialog/error-dialog.component';

@Injectable()
export class CustomeErrorHandler implements ErrorHandler {
  constructor(public dialog: MatDialog, private zone: NgZone) { }

  handleError(errorObj): void {
    let errMsg = '';
    if (errorObj.error) {
      if (errorObj.error.errors) {
        Object.values(errorObj.error.errors).forEach(err => {
          errMsg += err[0] + '\n';
        })
      } else {
        errMsg = errorObj.error.Message || errorObj.error;
      }
    } else {
      errMsg = errorObj.message;
    }

    if (this.zone) {
      this.zone.run(() => {
        this.dialog.open(ErrorDialogComponent, {
          width: '400px',
          data: {
            title: 'Error',
            message: errMsg
          }
        });
      });
      // alert(errMsg);
      console.error('Error log :', errorObj);
      // throw error;
    }
  }
}
