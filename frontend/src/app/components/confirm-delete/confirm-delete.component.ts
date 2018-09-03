import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'wr-confirm-dialog',
  templateUrl: './confirm-delete.component.html',
  styleUrls: ['./confirm-delete.component.scss']
})
export class ConfirmDeleteComponent {

  inProgress = false;

  objectName = '';
  deleteOperation: Function;

  constructor(
    private dialogRef: MatDialogRef<ConfirmDeleteComponent>,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    this.objectName = data.objectName;
    this.deleteOperation = data.deleteOperation;
  }

  doDelete(): void {
    this.inProgress = true;
    this.deleteOperation().subscribe(() => this.dialogRef.close(true));
  }

}
