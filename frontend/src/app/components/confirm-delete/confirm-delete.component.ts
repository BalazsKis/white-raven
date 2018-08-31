import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { NoteService } from '../../services/note.service';

@Component({
  selector: 'wr-confirm-dialog',
  templateUrl: './confirm-delete.component.html',
  styleUrls: ['./confirm-delete.component.scss']
})
export class ConfirmDeleteComponent {

  inProgress = false;

  constructor(
    private dialogRef: MatDialogRef<ConfirmDeleteComponent>,
    private noteService: NoteService,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) { }

  doDelete(): void {
    this.inProgress = true;
    this.noteService.deleteNoteById(this.data.note.id)
      .subscribe(() => this.dialogRef.close(true));
  }

}
