import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  isLoggedIn = false;

  constructor(private http: HttpClient) { }

  public registerUser(user: User): Observable<any> {
    const userRegistrationUrl = 'https://whiteraven.azurewebsites.net/api/users';
    return this.http.post(userRegistrationUrl, user);
  }

  public login(email: string, password: string): Observable<{ token: string }> {
    const tokenUrl = 'https://whiteraven.azurewebsites.net/api/token';
    return this.http.post<{ token: string }>(tokenUrl, { email, password });
  }

  public checkTokenValidity(onValid: Function, onInvalid: Function): void {
    const checkUrl = 'https://whiteraven.azurewebsites.net/api/contributions/mine';

    this.http.get(checkUrl)
      .subscribe(
        () => {
          this.isLoggedIn = true;
          onValid();
        },
        () => {
          this.isLoggedIn = false;
          onInvalid();
        }
      );
  }

  public isUserLoggedIn(): boolean {
    return this.isLoggedIn;
  }

}
