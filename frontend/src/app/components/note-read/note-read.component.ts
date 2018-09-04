import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog, MatSnackBar } from '@angular/material';
import { untilDestroyed } from 'ngx-take-until-destroy';

import { Note } from '../../models/note';
import { Contribution } from '../../models/contribution';
import { NoteService } from '../../services/note.service';
import { ConfirmDeleteComponent } from '../confirm-delete/confirm-delete.component';
import { ShareAddComponent } from '../share-add/share-add.component';
import { ShareListComponent } from '../share-list/share-list.component';

@Component({
  selector: 'wr-note-read',
  templateUrl: './note-read.component.html',
  styleUrls: ['./note-read.component.scss']
})
export class NoteReadComponent implements OnInit, OnDestroy {

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
    this.route.params
      .pipe(untilDestroyed(this))
      .subscribe(params => {
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
          .pipe(untilDestroyed(this))
          .subscribe(() => this.contribution = this.noteService.getContributionForNote(id));
      });
  }

  ngOnDestroy() {
  }

  addShare(): void {
    const dialogRef = this.dialog.open(ShareAddComponent,
      {
        data: {
          noteId: this.note.id
        },
        panelClass: 'full-width-dialog'
      });

    dialogRef.afterClosed()
      .pipe(untilDestroyed(this))
      .subscribe(shared => {
        if (shared) {
          this.showMessageInSnack('Note shared');
        }
      });
  }

  viewShare(): void {
    this.dialog.open(ShareListComponent,
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

    dialogRef.afterClosed()
      .pipe(untilDestroyed(this))
      .subscribe(isDeleted => {
        if (isDeleted) {
          this.router.navigate(['/app']);
          this.showMessageInSnack('Note deleted');
        }
      });
  }

  private showMessageInSnack(message: string) {
    this.snackBar.open(message, null, { duration: 1500 });
  }

}
