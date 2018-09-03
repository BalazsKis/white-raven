import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';

import { LoginService } from './login.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate {

  constructor(
    private loginService: LoginService,
    private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return this.checkLoggedIn();
  }

  private checkLoggedIn(): boolean {
    if (this.loginService.isUserLoggedIn()) {
      return true;
    }

    this.router.navigate(['']);
    return false;
  }

}
