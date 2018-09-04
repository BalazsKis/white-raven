import { Component, EventEmitter, OnInit, Output, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { untilDestroyed } from 'ngx-take-until-destroy';

import { NoteService } from '../../services/note.service';
import { Note } from '../../models/note';

@Component({
  selector: 'wr-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss']
})
export class ToolbarComponent implements OnInit, OnDestroy {

  @Output() toggleSidenav = new EventEmitter<void>();
  @Output() toggleTheme = new EventEmitter<void>();
  @Output() logOut = new EventEmitter<void>();

  constructor(private router: Router, private noteService: NoteService) { }

  ngOnInit() {
  }

  ngOnDestroy() {
  }

  createNote(): void {
    const n = new Note();
    n.title = 'New note';
    n.content = '';

    this.noteService.createNote(n)
      .pipe(untilDestroyed(this))
      .subscribe(createdNote => {
        if (createdNote && createdNote.id) { this.router.navigate(['/app/edit', createdNote.id]); }
      });
  }

}
