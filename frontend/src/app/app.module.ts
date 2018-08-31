import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { MaterialModule } from './shared/material.module';

import { AppComponent } from './app.component';
import { ToolbarComponent } from './components/toolbar/toolbar.component';
import { NoContentComponent } from './components/no-content/no-content.component';
import { SideNavComponent } from './components/side-nav/side-nav.component';
import { TokenInterceptor } from './auth/token-interceptor';
import { NoteListComponent } from './components/note-list/note-list.component';
import { NoteReadComponent } from './components/note-read/note-read.component';
import { NoteEditComponent } from './components/note-edit/note-edit.component';
import { TruncatePipe } from './pipes/truncate.pipe';
import { ConfirmDeleteComponent } from './components/confirm-delete/confirm-delete.component';
import { AddShareComponent } from './components/add-share/add-share.component';
import { ViewShareComponent } from './components/view-share/view-share.component';

@NgModule({
  declarations: [
    AppComponent,
    ToolbarComponent,
    NoContentComponent,
    SideNavComponent,
    NoteListComponent,
    NoteReadComponent,
    NoteEditComponent,
    TruncatePipe,
    ConfirmDeleteComponent,
    AddShareComponent,
    ViewShareComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    BrowserModule,
    BrowserAnimationsModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    AppRoutingModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent],
  entryComponents: [
    ConfirmDeleteComponent,
    AddShareComponent,
    ViewShareComponent
  ]
})
export class AppModule { }
