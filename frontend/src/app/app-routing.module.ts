import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { MainContentComponent } from './components/main-content/main-content.component';

const routes: Routes = [
  { path: ':id', component: MainContentComponent },
  { path: '', component: MainContentComponent },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
