import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';

import { NoteEditComponent } from '../components/note-edit/note-edit.component';
import { MatDialog } from '@angular/material';
import { Observable, from } from 'rxjs';
import { ConfirmLeaveComponent } from '../components/confirm-leave/confirm-leave.component';

@Injectable({
  providedIn: 'root'
})
export class NoteGuardService implements CanDeactivate<NoteEditComponent> {

  constructor(private dialog: MatDialog) { }

  canDeactivate(component: NoteEditComponent): Observable<boolean> {
    if (component.areChangesSaved()) {
      return from([true]);
    }

    const dialogRef = this.dialog.open(ConfirmLeaveComponent);
    return dialogRef.afterClosed();
  }

}
