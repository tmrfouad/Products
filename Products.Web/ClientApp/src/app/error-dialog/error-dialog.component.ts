import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { Component, Inject } from "@angular/core";

@Component({
  selector: 'app-error-dialog.component',
  template: `
    <h1 mat-dialog-title>{{ data.title }}</h1>
    <div mat-dialog-content>
      <p>{{ data.message }}</p>
    </div>
    <div mat-dialog-actions>
      <button class="btn btn-secondary ml-1" (click)="onOkClick()">Ok</button>
    </div>
  `
})
export class ErrorDialogComponent {
  constructor(public dialogRef: MatDialogRef<ErrorDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: { title: string, message: string }) { }

  onOkClick(): void {
    this.dialogRef.close();
  }
}
