import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog, MatSnackBar } from '@angular/material';

import { Note } from '../../models/note';
import { Contribution } from '../../models/contribution';
import { NoteService } from '../../services/note.service';
import { ConfirmDeleteComponent } from '../confirm-delete/confirm-delete.component';
import { AddShareComponent } from '../add-share/add-share.component';
import { ViewShareComponent } from '../view-share/view-share.component';

@Component({
  selector: 'wr-note-read',
  templateUrl: './note-read.component.html',
  styleUrls: ['./note-read.component.scss']
})
export class NoteReadComponent implements OnInit {

  note: Note;
  contribution: Contribution;

  public get canEdit(): boolean {
    return this.contribution.contributionType >= 2;
  }

  public get canShare(): boolean {
    return this.contribution.contributionType >= 3;
  }

  public get canDelete(): boolean {
    return this.contribution.contributionType >= 3;
  }

  constructor(
    private route: ActivatedRoute,
    private noteService: NoteService,
    private router: Router,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      const id = params['id'];

      this.noteService.allNotes
        .subscribe(notes => {
          if (notes && notes.length && id) {
            this.note = null;

            this.contribution = this.noteService.getContributionForNote(id);
            this.note = this.noteService.getNoteById(id);
          }
        });

      this.noteService.contributions
        .subscribe(() => this.contribution = this.noteService.getContributionForNote(id));
    });
  }

  addShare(): void {
    const dialogRef = this.dialog.open(AddShareComponent,
      {
        data: {
          noteId: this.note.id
        },
        panelClass: 'full-width-dialog'
      });

    dialogRef.afterClosed().subscribe(shared => {
      if (shared) {
        this.showMessageInSnack('Note shared');
      }
    });
  }

  viewShare(): void {
    this.dialog.open(ViewShareComponent,
      {
        data: {
          noteId: this.note.id,
          canRemoveShare: this.canDelete
        },
        panelClass: 'full-width-dialog'
      });
  }

  delete(): void {
    const dialogRef = this.dialog.open(ConfirmDeleteComponent,
      {
        data: {
          objectName: 'note',
          deleteOperation: () => this.noteService.deleteNoteById(this.note.id)
        }
      });

    dialogRef.afterClosed().subscribe(isDeleted => {
      if (isDeleted) {
        this.router.navigate(['']);
        this.showMessageInSnack('Note deleted');
      }
    });
  }

  private doDelete(): void {
    const id = this.note.id;
    this.note = null;

    this.noteService.deleteNoteById(id)
      .subscribe(() => {

      });
  }

  private showMessageInSnack(message: string) {
    this.snackBar.open(message, null, { duration: 1500 });
  }

}
