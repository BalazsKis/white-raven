import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Note } from '../../models/note';
import { Contribution } from '../../models/contribution';
import { NoteService } from '../../services/note.service';

@Component({
  selector: 'wr-note-read',
  templateUrl: './note-read.component.html',
  styleUrls: ['./note-read.component.scss']
})
export class NoteReadComponent implements OnInit {

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
    private noteService: NoteService
  ) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      const id = params['id'];
      this.note = null;

      this.noteService.allNotes.subscribe(notes => {
        if (notes && notes.length) {
          // setTimeout(() => {
            this.contribution = this.noteService.getContributionForNote(id);
            this.note = this.noteService.getNoteById(id);
            this.note.content = this.note.content.replace(/\n/g, '<br />');
          // }, 1500);
        }
      });

    });
  }

}
