import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { map, tap } from 'rxjs/operators';

import { Note } from '../models/note';
import { Response } from '../models/response';
import { Contribution } from '../models/contribution';


@Injectable({
  providedIn: 'root'
})
export class NoteService {

  private dataStore: {
    allNotes: Note[],
    myNotes: Note[],
    readOnlyNotes: Note[],
    editableNotes: Note[],
    contributions: Contribution[]
  };

  private _allNotes: BehaviorSubject<Note[]>;
  private _myNotes: BehaviorSubject<Note[]>;
  private _readOnlyNotes: BehaviorSubject<Note[]>;
  private _editableNotes: BehaviorSubject<Note[]>;
  private _contributions: BehaviorSubject<Contribution[]>;

  constructor(private http: HttpClient) {
    this.dataStore = {
      allNotes: [],
      myNotes: [],
      readOnlyNotes: [],
      editableNotes: [],
      contributions: []
    };

    this._allNotes = new BehaviorSubject<Note[]>([]);
    this._myNotes = new BehaviorSubject<Note[]>([]);
    this._readOnlyNotes = new BehaviorSubject<Note[]>([]);
    this._editableNotes = new BehaviorSubject<Note[]>([]);
    this._contributions = new BehaviorSubject<Contribution[]>([]);
  }

  get allNotes(): Observable<Note[]> {
    return this._allNotes.asObservable();
  }

  get myNotes(): Observable<Note[]> {
    return this._myNotes.asObservable();
  }

  get readOnlyNotes(): Observable<Note[]> {
    return this._readOnlyNotes.asObservable();
  }

  get editableNotes(): Observable<Note[]> {
    return this._editableNotes.asObservable();
  }

  get contributions(): Observable<Contribution[]> {
    return this._contributions.asObservable();
  }

  private downloadAllNotes(): void {
    const allNotesUrl = 'https://whiteraven.azurewebsites.net/api/notes/all';

    const s: Subscription = this.http.get<Response<Note[]>>(allNotesUrl)
      .subscribe(
        r => {
          this.dataStore.allNotes = r.data;
          this._allNotes.next([...this.dataStore.allNotes]);
        },
        error => this.handleDownloadError(error),
        () => s.unsubscribe()
      );
  }

  private downloadMyNotes(): void {
    const myNotesUrl = 'https://whiteraven.azurewebsites.net/api/notes/mine';

    const s: Subscription = this.http.get<Response<Note[]>>(myNotesUrl)
      .subscribe(
        r => {
          this.dataStore.myNotes = r.data;
          this._myNotes.next([...this.dataStore.myNotes]);
        },
        error => this.handleDownloadError(error),
        () => s.unsubscribe()
      );
  }

  private downloadReadOnlyNotes(): void {
    const readOnlyNotesUrl = 'https://whiteraven.azurewebsites.net/api/notes/shared/read';

    const s: Subscription = this.http.get<Response<Note[]>>(readOnlyNotesUrl)
      .subscribe(
        r => {
          this.dataStore.readOnlyNotes = r.data;
          this._readOnlyNotes.next([...this.dataStore.readOnlyNotes]);
        },
        error => this.handleDownloadError(error),
        () => s.unsubscribe()
      );
  }

  private downloadEditableNotes(): void {
    const editableNotesUrl = 'https://whiteraven.azurewebsites.net/api/notes/shared/write';

    const s: Subscription = this.http.get<Response<Note[]>>(editableNotesUrl)
      .subscribe(
        r => {
          this.dataStore.editableNotes = r.data;
          this._editableNotes.next([...this.dataStore.editableNotes]);
        },
        error => this.handleDownloadError(error),
        () => s.unsubscribe()
      );
  }

  public downloadContributions(): void {
    const contributionUrl = 'https://whiteraven.azurewebsites.net/api/contributions/all';

    const s: Subscription = this.http.get<Response<Contribution[]>>(contributionUrl)
      .subscribe(
        r => {
          this.dataStore.contributions = r.data;
          this._contributions.next([...this.dataStore.contributions]);
        },
        error => this.handleDownloadError(error),
        () => s.unsubscribe()
      );
  }

  private handleDownloadError(error: any) {
    // TODO: add better error handling
    console.log(`Failed to fetch every note: ${error}`);
  }

  public loadAll(): void {
    this.downloadAllNotes();
    this.downloadMyNotes();
    this.downloadReadOnlyNotes();
    this.downloadEditableNotes();
    this.downloadContributions();
  }

  public getNoteById(id: string): Note {
    return Object.assign({}, this.dataStore.allNotes.find(x => x.id === id));
  }

  public getContributionForNote(noteId: string) {
    return Object.assign({}, this.dataStore.contributions.find(x => x.noteId === noteId));
  }

  public updateNote(note: Note): Observable<Note> {
    const updateNoteUrl = `https://whiteraven.azurewebsites.net/api/notes/${note.id}`;

    return this.http.patch<Response<Note>>(updateNoteUrl, { title: note.title, content: note.content })
      .pipe(
        tap(r => {
          const indexInAll = this.dataStore.allNotes.findIndex(x => x.id === r.data.id);

          this.dataStore.allNotes[indexInAll] = r.data;
          this._allNotes.next([...this.dataStore.allNotes]);

          const indexInMy = this.dataStore.myNotes.findIndex(x => x.id === r.data.id);
          if (indexInMy >= 0) {
            this.dataStore.myNotes[indexInMy] = r.data;
            this._myNotes.next([...this.dataStore.myNotes]);
            return;
          }

          const indexInEditable = this.dataStore.editableNotes.findIndex(x => x.id === r.data.id);
          if (indexInEditable >= 0) {
            this.dataStore.editableNotes[indexInEditable] = r.data;
            this._editableNotes.next([...this.dataStore.editableNotes]);
            return;
          }

          throw new Error('Note was not found in the editable collections');
        }),
        map(r => r.data));
  }

  public createNote(note: Note): Observable<Note> {
    const createNoteUrl = 'https://whiteraven.azurewebsites.net/api/notes';

    return this.http.post<Response<Note>>(createNoteUrl, { title: note.title, content: note.content })
      .pipe(
        tap(r => {
          this.dataStore.allNotes.push(r.data);
          this._allNotes.next([...this.dataStore.allNotes]);

          this.dataStore.myNotes.push(r.data);
          this._myNotes.next([...this.dataStore.myNotes]);

          this.downloadContributions();
        }),
        map(r => r.data)
      );
  }

  public deleteNoteById(id: string): Observable<any> {
    const deleteNoteUrl = `https://whiteraven.azurewebsites.net/api/notes/${id}`;

    return this.http.delete(deleteNoteUrl)
      .pipe(
        tap(() => {
          const indexInAll = this.dataStore.allNotes.findIndex(x => x.id === id);
          this.dataStore.allNotes.splice(indexInAll, 1);
          this._allNotes.next([...this.dataStore.allNotes]);

          const indexInMy = this.dataStore.myNotes.findIndex(x => x.id === id);
          this.dataStore.myNotes.splice(indexInMy, 1);
          this._myNotes.next([...this.dataStore.myNotes]);
        })
      );
  }

}
