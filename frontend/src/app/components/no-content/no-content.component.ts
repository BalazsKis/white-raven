import { Component, OnInit } from '@angular/core';

import { NoteService } from '../../services/note.service';
import { Note } from '../../models/note';
import { Router } from '@angular/router';

@Component({
  selector: 'wr-no-content',
  templateUrl: './no-content.component.html',
  styleUrls: ['./no-content.component.scss']
})
export class NoContentComponent implements OnInit {

  myNotes: Note[] = [];

  constructor(private noteService: NoteService, private router: Router) { }

  ngOnInit(): void {
    this.noteService.myNotes.subscribe(n => {
      if (n && n.length) {
        this.router.navigate(['/app/read/', n[0].id]);
      }
    });
  }

}
