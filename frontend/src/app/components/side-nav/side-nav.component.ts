import { Component, OnInit, NgZone } from '@angular/core';
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

  notes: Observable<Note[]>;

  constructor(zone: NgZone, private noteService: NoteService) {
    this.mediaMatcher.addListener(mql => zone.run(() => this.mediaMatcher = mql));
  }

  ngOnInit() {
    this.notes = this.noteService.notes;
    this.noteService.loadAll();

    this.notes.subscribe(data => {
      console.log(data);
    });
  }

  isScreenSmall(): boolean {
    return this.mediaMatcher.matches;
  }

}
