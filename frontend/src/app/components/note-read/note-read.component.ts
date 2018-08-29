import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

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
    private noteService: NoteService,
    private router: Router
  ) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      const id = params['id'];

      this.noteService.allNotes
        .subscribe(notes => {
          if (notes && notes.length && id) {
            this.note = null;

            this.contribution = this.noteService.getContributionForNote(id);
            this.note = this.noteService.getNoteById(id);
            if (this.note.content) {
              this.note.content = this.note.content.replace(/\n/g, '<br />');
            }
          }
        });

      this.noteService.contributions
        .subscribe(() => this.contribution = this.noteService.getContributionForNote(id));
    });
  }

  delete(): void {
    const id = this.note.id;
    this.note = null;

    this.noteService.deleteNoteById(id)
      .subscribe(() => this.router.navigate(['']));
  }

}
