import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { User } from '../models/user';
import { Response } from '../models/response';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  public searchByEmail(emailFragment: string): Observable<User[]> {
    const userSearchByEmaulUrl = `https://whiteraven.azurewebsites.net/api/users/search/email/${emailFragment}`;
    return this.http.get<Response<User[]>>(userSearchByEmaulUrl).pipe(map(r => r.data));
  }
}
