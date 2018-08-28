import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

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
    private noteService: NoteService
  ) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      const id = params['id'];
      this.note = null;

      this.noteService.allNotes.subscribe(notes => {
        if (notes && notes.length) {
          // setTimeout(() => {
            this.note = this.noteService.getNoteById(id);
          // }, 1500);
        }
      });

    });
  }

  saveChanges() {
    this.noteService.updateNote(this.note);
  }

}
