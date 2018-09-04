import { Component, OnInit } from '@angular/core';
import { OverlayContainer } from '@angular/cdk/overlay';
import { StorageService } from './services/storage.service';

@Component({
  selector: 'wr-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  constructor(
    private overlay: OverlayContainer,
    private storageService: StorageService) { }

  ngOnInit(): void {
    let theme = 'light-custom-theme';

    if (this.storageService.hasTheme()) {
      theme = this.storageService.getTheme();
    } else {
      this.storageService.setTheme(theme);
    }

    document.body.classList.add(theme, 'mat-app-background');
    this.overlay.getContainerElement().classList.add(theme);
  }

}
