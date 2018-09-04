import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class StorageService {

  private tokenKey = 'id_token';
  private emailKey = 'id:email';
  private themeKey = 'display_theme';

  constructor() { }

  public hasToken(): boolean { return this.hasItem(this.tokenKey); }
  public setToken(token: string): void { this.setItem(this.tokenKey, token); }
  public getToken(): string { return this.getItem(this.tokenKey); }
  public removeToken(): void { this.removeItem(this.tokenKey); }

  public hasEmail(): boolean { return this.hasItem(this.emailKey); }
  public setEmail(email: string): void { this.setItem(this.emailKey, email); }
  public getEmail(): string { return this.getItem(this.emailKey); }
  public removeEmail(): void { this.removeItem(this.emailKey); }

  public hasTheme(): boolean { return this.hasItem(this.themeKey); }
  public setTheme(theme: string): void { this.setItem(this.themeKey, theme); }
  public getTheme(): string { return this.getItem(this.themeKey); }
  public removeTheme(): void { this.removeItem(this.themeKey); }

  public hasItem(key: string): boolean {
    const t = localStorage.getItem(key);
    return t && t.length > 0;
  }

  public setItem(key: string, item: any): void {
    localStorage.setItem(key, item);
  }

  public getItem(key: string): string {
    const t = localStorage.getItem(key);
    return t && t.length > 0 ? t : null;
  }

  public removeItem(key: string): void {
    localStorage.removeItem(key);
  }

}
