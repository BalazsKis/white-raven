import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Note } from '../models/note';
import { Response } from '../models/response';
import { BehaviorSubject, Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class NoteService {

  private dataStore: { notes: Note[] };
  private _notes: BehaviorSubject<Note[]>;

  constructor(private http: HttpClient) {
    this.dataStore = { notes: [] };
    this._notes = new BehaviorSubject<Note[]>([]);
  }

  get notes(): Observable<Note[]> {
    return this._notes.asObservable();
  }

  loadAll() {
    const notesUrl = 'https://whiteraven.azurewebsites.net/api/notes/all';

    return this.http.get<Response<Note[]>>(notesUrl)
      .subscribe(r => {
        this.dataStore.notes = r.data;
        this._notes.next(Object.assign({}, this.dataStore).notes);
      }, error => {
        console.log(`Failed to fetch notes: ${error}`);
      });
  }

}
