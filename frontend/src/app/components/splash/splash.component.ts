import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { StorageService } from '../../services/storage.service';
import { LoginService } from '../../services/login.service';

@Component({
  selector: 'wr-splash',
  templateUrl: './splash.component.html',
  styleUrls: ['./splash.component.scss']
})
export class SplashComponent implements OnInit {

  constructor(
    private router: Router,
    private storageService: StorageService,
    private loginService: LoginService
  ) { }

  ngOnInit() {
    if (this.storageService.hasToken()) {
      this.loginService.checkTokenValidity(
        () => this.toApp(),
        () => this.toLoginPage()
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
