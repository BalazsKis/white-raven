import { Component, OnInit } from '@angular/core';
import { OverlayContainer } from '@angular/cdk/overlay';

@Component({
  selector: 'wr-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  constructor(private overlay: OverlayContainer) { }

  ngOnInit(): void {
    document.body.classList.add('light-custom-theme', 'mat-app-background');
    this.overlay.getContainerElement().classList.add('light-custom-theme');
  }

}
