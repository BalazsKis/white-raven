import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormControl } from '@angular/forms';
import { untilDestroyed } from 'ngx-take-until-destroy';

import { Note } from '../../models/note';
import { NoteService } from '../../services/note.service';

@Component({
  selector: 'wr-note-edit',
  templateUrl: './note-edit.component.html',
  styleUrls: ['./note-edit.component.scss']
})
export class NoteEditComponent implements OnInit, OnDestroy {

  note: Note;
  editForm: FormGroup;

  private isSaved = false;

  constructor(
    private route: ActivatedRoute,
    private noteService: NoteService,
    private router: Router
  ) { }

  ngOnInit() {
    this.route.params
    .pipe(untilDestroyed(this))
    .subscribe(params => {
      const id = params['id'];
      this.note = null;

      this.noteService.allNotes
      .pipe(untilDestroyed(this))
      .subscribe(notes => {
        if (notes && notes.length) {
          this.note = this.noteService.getNoteById(id);

          this.editForm = new FormGroup({
            title: new FormControl(this.note.title),
            content: new FormControl(this.note.content ? this.note.content.replace(/<br\s*[\/]?>/gi, '\n') : '')
          });
        }
      });
    });
  }

  public areChangesSaved(): boolean {
    return this.isSaved || this.editForm.pristine;
  }

  saveChanges() {
    const n = new Note();
    n.id = this.note.id;
    n.title = this.editForm.controls.title.value;
    n.content = this.editForm.controls.content.value
      ? this.editForm.controls.content.value.replace(/\n/g, '<br>')
      : '';

    this.noteService.updateNote(n)
    .pipe(untilDestroyed(this))
      .subscribe(r => {
        this.isSaved = true;
        this.router.navigate(['/app/read', this.note.id]);
      });

    this.note = null;
  }

  ngOnDestroy() {
  }

}
