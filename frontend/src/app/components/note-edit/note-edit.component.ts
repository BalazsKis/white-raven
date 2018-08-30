import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Note } from '../../models/note';
import { NoteService } from '../../services/note.service';

@Component({
  selector: 'wr-note-edit',
  templateUrl: './note-edit.component.html',
  styleUrls: ['./note-edit.component.scss']
})
export class NoteEditComponent implements OnInit {

  note: Note;

  constructor(
    private route: ActivatedRoute,
    private noteService: NoteService,
    private router: Router
  ) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      const id = params['id'];
      this.note = null;

      this.noteService.allNotes.subscribe(notes => {
        if (notes && notes.length) {
          this.note = this.noteService.getNoteById(id);

          if (this.note.content) {
            this.note.content = this.note.content.replace(/<br\s*[\/]?>/gi, '\n');
          }
        }
      });

    });
  }

  saveChanges() {
    const n = Object.assign(new Note(), this.note);
    n.content = this.note.content.replace(/\n/g, '<br>');

    this.noteService.updateNote(n)
      .subscribe(r => this.router.navigate(['/read', this.note.id]));

    this.note = null;
  }

}
