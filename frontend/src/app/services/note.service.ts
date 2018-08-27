import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Note } from '../models/note';
import { Response } from '../models/response';
import { BehaviorSubject, Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class NoteService {

  private dataStore: { allNotes: Note[], myNotes: Note[], readOnlyNotes: Note[], editableNotes: Note[] };

  private _allNotes: BehaviorSubject<Note[]>;
  private _myNotes: BehaviorSubject<Note[]>;
  private _readOnlyNotes: BehaviorSubject<Note[]>;
  private _editableNotes: BehaviorSubject<Note[]>;

  constructor(private http: HttpClient) {
    this.dataStore = { allNotes: [], myNotes: [], readOnlyNotes: [], editableNotes: [] };

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

  noteById(id: string): Note {
    return this.dataStore.allNotes.find(x => x.id === id);
  }

  private downloadAllNotes(): void {
    const allNotesUrl = 'https://whiteraven.azurewebsites.net/api/notes/all';

    this.http.get<Response<Note[]>>(allNotesUrl)
      .subscribe(r => {
        this.dataStore.allNotes = r.data;
        this._allNotes.next(Object.assign({}, this.dataStore).allNotes);
      }, error => this.HandleDownloadError(error));
  }

  private downloadMyNotes(): void {
    const myNotesUrl = 'https://whiteraven.azurewebsites.net/api/notes/mine';

    this.http.get<Response<Note[]>>(myNotesUrl)
      .subscribe(r => {
        this.dataStore.myNotes = r.data;
        this._myNotes.next(Object.assign({}, this.dataStore).myNotes);
      }, error => this.HandleDownloadError(error));
  }

  private downloadReadOnlyNotes(): void {
    const readOnlyNotesUrl = 'https://whiteraven.azurewebsites.net/api/notes/shared/read';

    this.http.get<Response<Note[]>>(readOnlyNotesUrl)
      .subscribe(r => {
        this.dataStore.readOnlyNotes = r.data;
        this._readOnlyNotes.next(Object.assign({}, this.dataStore).readOnlyNotes);
      }, error => this.HandleDownloadError(error));
  }

  private downloadEditableNotes(): void {
    const editableNotesUrl = 'https://whiteraven.azurewebsites.net/api/notes/shared/write';

    this.http.get<Response<Note[]>>(editableNotesUrl)
      .subscribe(r => {
        this.dataStore.editableNotes = r.data;
        this._editableNotes.next(Object.assign({}, this.dataStore).editableNotes);
      }, error => this.HandleDownloadError(error));
  }

  private HandleDownloadError(error: any) {
    console.log(`Failed to fetch every note: ${error}`);
  }

  public loadAll(): void {
    this.downloadAllNotes();
    this.downloadMyNotes();
    this.downloadReadOnlyNotes();
    this.downloadEditableNotes();
  }

}
