import { Component, OnInit, NgZone, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MatSidenav } from '@angular/material';
import { OverlayContainer } from '@angular/cdk/overlay';

import { StorageService } from '../../services/storage.service';

const SMALL_WIDTH_BREAKPOINT = 720;

@Component({
  selector: 'wr-sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.scss']
})
export class SidenavComponent implements OnInit {

  private mediaMatcher: MediaQueryList = matchMedia(`(max-width: ${SMALL_WIDTH_BREAKPOINT}px)`);

  constructor(
    zone: NgZone,
    private router: Router,
    private storageService: StorageService,
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
    this.switchThemeOnElementClassList(this.overlay.getContainerElement().classList);
    this.switchThemeOnElementClassList(document.body.classList);
  }

  private switchThemeOnElementClassList(classList: DOMTokenList): void {
    if (classList.contains('custom-theme')) {
      classList.remove('custom-theme');
      classList.add('light-custom-theme');
      return;
    }

    if (classList.contains('light-custom-theme')) {
      classList.remove('light-custom-theme');
      classList.add('custom-theme');
      return;
    }

    classList.add('light-custom-theme');
  }


  logOut(): void {
    this.storageService.removeEmail();
    this.storageService.removeToken();

    this.router.navigate(['/login']);
  }

}
