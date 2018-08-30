import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { debounceTime, distinctUntilChanged, switchMap, filter } from 'rxjs/operators';

import { UserService } from '../../services/user.service';
import { Observable } from 'rxjs';
import { User } from '../../models/user';
import { MatDialogRef } from '@angular/material';

@Component({
  selector: 'wr-add-share',
  templateUrl: './add-share.component.html',
  styleUrls: ['./add-share.component.scss']
})
export class AddShareComponent implements OnInit {

  emailField: FormControl = new FormControl();
  nameForm: FormGroup = new FormGroup({ firstName: new FormControl(), lastName: new FormControl() });

  emailSearchResults: Observable<User[]> = new Observable<User[]>();
  nameSearchResults: Observable<User[]> = new Observable<User[]>();

  constructor(private userService: UserService, private dialogRef: MatDialogRef<AddShareComponent>) { }

  ngOnInit() {
    this.emailSearchResults = this.emailField.valueChanges
      .pipe(
        debounceTime(1000),
        distinctUntilChanged(),
        switchMap((query) => this.userService.searchByEmail(query))
      );

    this.nameSearchResults = this.nameForm.valueChanges
      .pipe(
        debounceTime(1000),
        distinctUntilChanged(),
        switchMap((query) => this.userService.searchByName(query.firstName, query.lastName))
      );
  }

  share(): void {
    // TODO: call sharing service here.
    this.dialogRef.close(true);
  }

}
