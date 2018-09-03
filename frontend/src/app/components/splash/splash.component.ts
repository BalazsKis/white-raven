import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { TokenService } from '../../services/token.service';
import { LoginService } from '../../services/login.service';

@Component({
  selector: 'wr-splash',
  templateUrl: './splash.component.html',
  styleUrls: ['./splash.component.scss']
})
export class SplashComponent implements OnInit {

  constructor(
    private router: Router,
    private tokenService: TokenService,
    private loginService: LoginService
  ) { }

  ngOnInit() {
    if (this.tokenService.hasToken()) {
      this.loginService.checkTokenValidity(
        () => this.toApp(),
        () => {
          this.tokenService.removeToken();
          this.toLoginPage();
        }
      );
    } else {
      this.toLoginPage();
    }
  }

  private toLoginPage(): void {
    this.router.navigate(['/login']);
  }

  private toApp() {
    this.router.navigate(['/app']);
  }

}
