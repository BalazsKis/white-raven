import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Note } from '../../models/note';
import { NoteService } from '../../services/note.service';

@Component({
  selector: 'wr-main-content',
  templateUrl: './main-content.component.html',
  styleUrls: ['./main-content.component.scss']
})
export class MainContentComponent implements OnInit {

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
            this.note = this.noteService.noteById(id);
          // }, 1500);
        }
      });

    });
  }

}
