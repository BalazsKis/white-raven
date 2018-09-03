import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { NoContentComponent } from './components/no-content/no-content.component';
import { NoteReadComponent } from './components/note-read/note-read.component';
import { NoteEditComponent } from './components/note-edit/note-edit.component';
import { SidenavComponent } from './components/sidenav/sidenav.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { SplashComponent } from './components/splash/splash.component';

const routes: Routes = [
  {
    path: 'app', component: SidenavComponent, children: [
      { path: 'read/:id', component: NoteReadComponent },
      { path: 'edit/:id', component: NoteEditComponent },
      { path: '', component: NoContentComponent }
    ]
  },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: '', component: SplashComponent },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
