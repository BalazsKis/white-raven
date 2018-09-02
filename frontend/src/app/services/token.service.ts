import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  private tokenKey = 'id_token';

  constructor() { }

  public hasToken(): boolean {
    const t = localStorage.getItem(this.tokenKey);
    return t && t.length > 0;
  }

  public setToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  public getToken(): string {
    const t = localStorage.getItem(this.tokenKey);
    return t && t.length > 0 ? t : null;
  }

  public removeToken(): void {
    localStorage.removeItem(this.tokenKey);
  }

}
