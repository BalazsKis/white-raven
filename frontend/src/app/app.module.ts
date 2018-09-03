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
import { SidenavComponent } from './components/sidenav/sidenav.component';
import { TokenInterceptor } from './auth/token-interceptor';
import { NoteListComponent } from './components/note-list/note-list.component';
import { NoteReadComponent } from './components/note-read/note-read.component';
import { NoteEditComponent } from './components/note-edit/note-edit.component';
import { TruncatePipe } from './pipes/truncate.pipe';
import { ConfirmDeleteComponent } from './components/confirm-delete/confirm-delete.component';
import { ShareAddComponent } from './components/share-add/share-add.component';
import { ShareListComponent } from './components/share-list/share-list.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { SplashComponent } from './components/splash/splash.component';

@NgModule({
  declarations: [
    AppComponent,
    ToolbarComponent,
    NoContentComponent,
    SidenavComponent,
    NoteListComponent,
    NoteReadComponent,
    NoteEditComponent,
    TruncatePipe,
    ConfirmDeleteComponent,
    ShareAddComponent,
    ShareListComponent,
    LoginComponent,
    RegisterComponent,
    SplashComponent
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
    ShareAddComponent,
    ShareListComponent
  ]
})
export class AppModule { }
