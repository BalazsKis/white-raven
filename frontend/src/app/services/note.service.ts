import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';

import { Note } from '../models/note';
import { Response } from '../models/response';
import { Contribution } from '../models/contribution';


@Injectable({
  providedIn: 'root'
})
export class NoteService {

  private dataStore: { allNotes: Note[], myNotes: Note[], readOnlyNotes: Note[], editableNotes: Note[], contributions: Contribution[] };

  private _allNotes: BehaviorSubject<Note[]>;
  private _myNotes: BehaviorSubject<Note[]>;
  private _readOnlyNotes: BehaviorSubject<Note[]>;
  private _editableNotes: BehaviorSubject<Note[]>;

  constructor(private http: HttpClient) {
    this.dataStore = { allNotes: [], myNotes: [], readOnlyNotes: [], editableNotes: [], contributions: [] };

    this._allNotes = new BehaviorSubject<Note[]>([]);
    this._myNotes = new BehaviorSubject<Note[]>([]);
    this._readOnlyNotes = new BehaviorSubject<Note[]>([]);
    this._editableNotes = new BehaviorSubject<Note[]>([]);
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

  private downloadAllNotes(): void {
    const allNotesUrl = 'https://whiteraven.azurewebsites.net/api/notes/all';

    this.http.get<Response<Note[]>>(allNotesUrl)
      .subscribe(r => {
        this.dataStore.allNotes = r.data;
        this._allNotes.next(Object.assign({}, this.dataStore).allNotes);
      }, error => this.handleDownloadError(error));
  }

  private downloadMyNotes(): void {
    const myNotesUrl = 'https://whiteraven.azurewebsites.net/api/notes/mine';

    this.http.get<Response<Note[]>>(myNotesUrl)
      .subscribe(r => {
        this.dataStore.myNotes = r.data;
        this._myNotes.next(Object.assign({}, this.dataStore).myNotes);
      }, error => this.handleDownloadError(error));
  }

  private downloadReadOnlyNotes(): void {
    const readOnlyNotesUrl = 'https://whiteraven.azurewebsites.net/api/notes/shared/read';

    this.http.get<Response<Note[]>>(readOnlyNotesUrl)
      .subscribe(r => {
        this.dataStore.readOnlyNotes = r.data;
        this._readOnlyNotes.next(Object.assign({}, this.dataStore).readOnlyNotes);
      }, error => this.handleDownloadError(error));
  }

  private downloadEditableNotes(): void {
    const editableNotesUrl = 'https://whiteraven.azurewebsites.net/api/notes/shared/write';

    this.http.get<Response<Note[]>>(editableNotesUrl)
      .subscribe(r => {
        this.dataStore.editableNotes = r.data;
        this._editableNotes.next(Object.assign({}, this.dataStore).editableNotes);
      }, error => this.handleDownloadError(error));
  }

  public downloadContributions(): void {
    const contributionUrl = 'https://whiteraven.azurewebsites.net/api/contributions/all';

    this.http.get<Response<Contribution[]>>(contributionUrl)
      .subscribe(r => {
        this.dataStore.contributions = r.data;
      }, error => this.handleDownloadError(error));
  }

  private handleDownloadError(error: any) {
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

  public updateNote(note: Note): void {
    const updateNoteUrl = `https://whiteraven.azurewebsites.net/api/notes/${note.id}`;

    this.http.patch<Response<Note>>(updateNoteUrl, { title: note.title, content: note.content })
      .subscribe(r => {
        const indexInAll = this.dataStore.allNotes.findIndex(x => x.id === r.data.id);

        this.dataStore.allNotes[indexInAll] = r.data;
        this._allNotes.next(Object.assign({}, this.dataStore).allNotes);

        const indexInMy = this.dataStore.myNotes.findIndex(x => x.id === r.data.id);
        if (indexInMy >= 0) {
          this.dataStore.myNotes[indexInMy] = r.data;
          this._myNotes.next(Object.assign({}, this.dataStore).myNotes);
          return;
        }

        const indexInEditable = this.dataStore.editableNotes.findIndex(x => x.id === r.data.id);
        if (indexInEditable >= 0) {
          this.dataStore.editableNotes[indexInEditable] = r.data;
          this._editableNotes.next(Object.assign({}, this.dataStore).editableNotes);
          return;
        }

        throw new Error('Note was not found in the editable collections');
      }, error => this.handleDownloadError(error));
  }

}
