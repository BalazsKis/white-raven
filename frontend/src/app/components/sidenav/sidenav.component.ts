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

  private lightThemeClassName = 'light-custom-theme';
  private darkThemeClassName = 'custom-theme';

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
    if (classList.contains(this.darkThemeClassName)) {
      classList.remove(this.darkThemeClassName);
      classList.add(this.lightThemeClassName);

      this.storageService.setTheme(this.lightThemeClassName);
      return;
    }

    if (classList.contains(this.lightThemeClassName)) {
      classList.remove(this.lightThemeClassName);
      classList.add(this.darkThemeClassName);

      this.storageService.setTheme(this.darkThemeClassName);
      return;
    }

    classList.add(this.lightThemeClassName);
    this.storageService.setTheme(this.lightThemeClassName);
  }


  logOut(): void {
    this.storageService.removeEmail();
    this.storageService.removeToken();
    this.storageService.removeTheme();

    this.router.navigate(['/login']);
  }

}
