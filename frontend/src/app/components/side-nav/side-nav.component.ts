import { Component, OnInit, NgZone, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MatSidenav } from '@angular/material';
import { OverlayContainer } from '@angular/cdk/overlay';

import { TokenService } from '../../services/token.service';

const SMALL_WIDTH_BREAKPOINT = 720;

@Component({
  selector: 'wr-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.scss']
})
export class SideNavComponent implements OnInit {

  private mediaMatcher: MediaQueryList = matchMedia(`(max-width: ${SMALL_WIDTH_BREAKPOINT}px)`);

  constructor(
    zone: NgZone,
    private router: Router,
    private tokenService: TokenService,
    private overlay: OverlayContainer) {
    this.mediaMatcher.addListener(mql => zone.run(() => this.mediaMatcher = mql));
  }

  @ViewChild(MatSidenav) sidenav: MatSidenav;

  ngOnInit() {
    this.router.events.subscribe(() => {
      if (this.isScreenSmall()) {
        this.sidenav.close();
      }
    });
  }

  isScreenSmall(): boolean {
    return this.mediaMatcher.matches;
  }

  toggleTheme(): void {
    if (this.overlay.getContainerElement().classList.contains('custom-theme')) {
      this.overlay.getContainerElement().classList.remove('custom-theme');
      this.overlay.getContainerElement().classList.add('light-custom-theme');
    } else if (this.overlay.getContainerElement().classList.contains('light-custom-theme')) {
      this.overlay.getContainerElement().classList.remove('light-custom-theme');
      this.overlay.getContainerElement().classList.add('custom-theme');
    } else {
      this.overlay.getContainerElement().classList.add('light-custom-theme');
    }

    if (document.body.classList.contains('custom-theme')) {
      document.body.classList.remove('custom-theme');
      document.body.classList.add('light-custom-theme');
    } else if (document.body.classList.contains('light-custom-theme')) {
      document.body.classList.remove('light-custom-theme');
      document.body.classList.add('custom-theme');
    } else {
      document.body.classList.add('light-custom-theme');
    }
  }

  logOut(): void {
    this.tokenService.removeToken();
    this.router.navigate(['/login']);
  }

}
