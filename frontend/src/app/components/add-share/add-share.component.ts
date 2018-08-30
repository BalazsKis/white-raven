import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged, switchMap, filter } from 'rxjs/operators';

import { UserService } from '../../services/user.service';
import { Observable } from 'rxjs';
import { User } from '../../models/user';

@Component({
  selector: 'wr-add-share',
  templateUrl: './add-share.component.html',
  styleUrls: ['./add-share.component.scss']
})
export class AddShareComponent implements OnInit {

  emailField: FormControl = new FormControl();

  results: Observable<User[]> = new Observable<User[]>();

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.results = this.emailField.valueChanges
    .pipe(
      filter(x => x.length >= 3),
      debounceTime(1000),
      distinctUntilChanged(),
      switchMap((query) =>  this.userService.searchByEmail(query))
    );
  }

}
