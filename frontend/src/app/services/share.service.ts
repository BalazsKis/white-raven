import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, forkJoin, pipe } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';

import { Share } from '../models/share';
import { User } from '../models/user';
import { Contribution } from '../models/contribution';
import { Response } from '../models/response';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class ShareService {

  constructor(
    private http: HttpClient,
    private userService: UserService) { }

  public getSharesForNote(noteId: string): Observable<Share[]> {
    const contributionsUrl = `https://whiteraven.azurewebsites.net/api/contributions/to/note/${noteId}`;

    return this.http.get<Response<Contribution[]>>(contributionsUrl)
      .pipe(
        map(r => r.data),
        switchMap(cs =>
          forkJoin(
            cs.map(contribution => {
              const s = new Share();
              s.contributionType = contribution.contributionType;
              return this.userService.getUserByEmail(contribution.userId)
                .pipe(
                  map(u => {
                    s.user = u;
                    return s;
                  }));
            })
          )
        )
      );
  }

  public deleteShare(noteId: string, user: User, contributionType: number): Observable<any> {
    const contributionDeleteUrl = `https://whiteraven.azurewebsites.net/api/contributions`;

    const contribution = new Contribution();
    contribution.userId = user.email;
    contribution.noteId = noteId;
    contribution.contributionType = contributionType;

    const options = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
      body: contribution,
    };

    return this.http.delete(contributionDeleteUrl, options);
  }

  public createShare(noteId: string, user: User, contributionType: number): Observable<any> {
    const contributionCreateUrl = `https://whiteraven.azurewebsites.net/api/contributions`;

    const contribution = new Contribution();
    contribution.userId = user.email;
    contribution.noteId = noteId;
    contribution.contributionType = contributionType;

    return this.http.post(contributionCreateUrl, contribution);
  }

}
