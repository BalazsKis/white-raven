import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, from } from 'rxjs';
import { map } from 'rxjs/operators';

import { User } from '../models/user';
import { Response } from '../models/response';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  emptyResult: User[][] = [[]];

  constructor(private http: HttpClient) { }

  public getUserByEmail(fullEmailAddress: string): Observable<User> {
    const getUserUrl = `https://whiteraven.azurewebsites.net/api/users/${fullEmailAddress}`;
    return this.http.get<Response<User>>(getUserUrl).pipe(map(r => r.data));
  }

  public searchByEmail(emailFragment: string): Observable<User[]> {
    if (!emailFragment || emailFragment.length < 3) {
      return from(this.emptyResult);
    }

    const userSearchByEmailUrl = `https://whiteraven.azurewebsites.net/api/users/search/email/${emailFragment}`;
    return this.http.get<Response<User[]>>(userSearchByEmailUrl).pipe(map(r => r.data));
  }

  public searchByName(firstName?: string, lastName?: string): Observable<User[]> {
    if ((!firstName || firstName.length < 3) && (!lastName || lastName.length < 3)) {
      return from(this.emptyResult);
    }

    if (!firstName || firstName.length < 3) {
      return this.searchByLastName(lastName);
    }

    if (!lastName || lastName.length < 3) {
      return this.searchByFirstName(firstName);
    }

    const userSearchByName = `https://whiteraven.azurewebsites.net//api/users/search/fullname/${firstName}/${lastName}`;
    return this.http.get<Response<User[]>>(userSearchByName).pipe(map(r => r.data));
  }

  private searchByFirstName(firstName: string): Observable<User[]> {
    const userSearchByFirstName = `https://whiteraven.azurewebsites.net/api/users/search/firstname/${firstName}`;
    return this.http.get<Response<User[]>>(userSearchByFirstName).pipe(map(r => r.data));
  }

  private searchByLastName(lastName: string): Observable<User[]> {
    const userSearchByLastName = `https://whiteraven.azurewebsites.net/api/users/search/lastname/${lastName}`;
    return this.http.get<Response<User[]>>(userSearchByLastName).pipe(map(r => r.data));
  }

}
