import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { User } from '../models/user';
import { StorageService } from './storage.service';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  isLoggedIn = false;

  constructor(
    private http: HttpClient,
    private storageService: StorageService) { }

  public registerUser(user: User): Observable<any> {
    const userRegistrationUrl = 'https://whiteraven.azurewebsites.net/api/users';
    return this.http.post(userRegistrationUrl, user);
  }

  public login(email: string, password: string, onValid: Function, onInvalid: Function): void {
    const tokenUrl = 'https://whiteraven.azurewebsites.net/api/token';
    this.http.post<{ token: string }>(tokenUrl, { email, password }).subscribe(
      r => {
        this.storageService.setEmail(email);
        this.storageService.setToken(r.token);
        onValid(r);
      },
      error => {
        this.clearLoginInfoFromStorage();
        onInvalid(error);
      }
    );
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
          this.clearLoginInfoFromStorage();
          onInvalid();
        }
      );
  }

  public isUserLoggedIn(): boolean {
    return this.isLoggedIn;
  }

  private clearLoginInfoFromStorage(): void {
    this.storageService.removeEmail();
    this.storageService.removeToken();
  }

}
