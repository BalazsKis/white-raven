import { Component, OnInit, NgZone, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MatSidenav } from '@angular/material';
import { Observable } from 'rxjs';

import { Note } from '../../models/note';
import { NoteService } from '../../services/note.service';

const SMALL_WIDTH_BREAKPOINT = 720;

@Component({
  selector: 'wr-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.scss']
})
export class SideNavComponent implements OnInit {

  private mediaMatcher: MediaQueryList = matchMedia(`(max-width: ${SMALL_WIDTH_BREAKPOINT}px)`);

  myNotes: Observable<Note[]>;
  readOnlyNotes: Observable<Note[]>;
  editableNotes: Observable<Note[]>;

  constructor(
    zone: NgZone,
    private noteService: NoteService,
    private router: Router) {
    this.mediaMatcher.addListener(mql => zone.run(() => this.mediaMatcher = mql));
  }

  @ViewChild(MatSidenav) sidenav: MatSidenav;

  ngOnInit() {
    this.myNotes = this.noteService.myNotes;
    this.readOnlyNotes = this.noteService.readOnlyNotes;
    this.editableNotes = this.noteService.editableNotes;

    this.noteService.loadAll();

    this.myNotes.subscribe(data => {
      if (data && data.length && this.router.url === '/') {
        this.router.navigate(['/', data[0].id]);
      }
    });

    this.router.events.subscribe(() => {
      if (this.isScreenSmall()) {
        this.sidenav.close();
      }
    });
  }

  isScreenSmall(): boolean {
    return this.mediaMatcher.matches;
  }

}
