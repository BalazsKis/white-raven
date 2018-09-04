import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Subscription } from 'rxjs';

import { User } from '../models/user';
import { StorageService } from './storage.service';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private isLoggedIn = false;

  public get isUserLoggedIn(): boolean {
    return this.isLoggedIn;
  }

  constructor(
    private http: HttpClient,
    private storageService: StorageService) { }

  public registerUser(user: User, onSuccess: Function, onError: Function): void {
    const userRegistrationUrl = 'https://whiteraven.azurewebsites.net/api/users';
    const s: Subscription =  this.http.post(userRegistrationUrl, user)
    .subscribe(
      r => onSuccess(r),
      error => onError(error),
      () => s.unsubscribe()
    );
  }

  public login(email: string, password: string, onValid: Function, onInvalid: Function): void {
    const tokenUrl = 'https://whiteraven.azurewebsites.net/api/token';
    const s: Subscription = this.http.post<{ token: string }>(tokenUrl, { email, password })
      .subscribe(
        r => {
          this.storageService.setEmail(email);
          this.storageService.setToken(r.token);
          onValid(r);
        },
        error => {
          this.clearLoginInfoFromStorage();
          onInvalid(error);
        },
        () => s.unsubscribe()
      );
  }

  public checkTokenValidity(onValid: Function, onInvalid: Function): void {
    const checkUrl = 'https://whiteraven.azurewebsites.net/api/contributions/mine';

    const s: Subscription = this.http.get(checkUrl)
      .subscribe(
        () => {
          this.isLoggedIn = true;
          onValid();
        },
        () => {
          this.isLoggedIn = false;
          this.clearLoginInfoFromStorage();
          onInvalid();
        },
        () => s.unsubscribe()
      );
  }

  private clearLoginInfoFromStorage(): void {
    this.storageService.removeEmail();
    this.storageService.removeToken();
  }

}
