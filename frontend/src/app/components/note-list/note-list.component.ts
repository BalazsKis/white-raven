import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

import { Note } from '../../models/note';
import { NoteService } from '../../services/note.service';

@Component({
  selector: 'wr-note-list',
  templateUrl: './note-list.component.html',
  styleUrls: ['./note-list.component.scss']
})
export class NoteListComponent implements OnInit {

  myNotes: Observable<Note[]>;
  readOnlyNotes: Observable<Note[]>;
  editableNotes: Observable<Note[]>;

  constructor(private noteService: NoteService, private router: Router) { }

  ngOnInit() {
    this.myNotes = this.noteService.myNotes;
    this.readOnlyNotes = this.noteService.readOnlyNotes;
    this.editableNotes = this.noteService.editableNotes;

    this.noteService.loadAll();
  }

}
