import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { NoContentComponent } from './components/no-content/no-content.component';
import { NoteReadComponent } from './components/note-read/note-read.component';
import { NoteEditComponent } from './components/note-edit/note-edit.component';

const routes: Routes = [
  { path: 'read/:id', component: NoteReadComponent },
  { path: 'edit/:id', component: NoteEditComponent },
  { path: '', component: NoContentComponent },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
