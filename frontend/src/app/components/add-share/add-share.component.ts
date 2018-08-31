import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialogRef } from '@angular/material';
import { debounceTime, distinctUntilChanged, switchMap, tap } from 'rxjs/operators';

import { User } from '../../models/user';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'wr-add-share',
  templateUrl: './add-share.component.html',
  styleUrls: ['./add-share.component.scss']
})
export class AddShareComponent implements OnInit {

  emailField: FormControl = new FormControl();
  nameForm: FormGroup = new FormGroup({ firstName: new FormControl(), lastName: new FormControl() });

  emailSearchResults: User[] = [];
  nameSearchResults: User[] = [];

  isEmailResultLoading = false;
  isEmailResultEmpty = false;
  isNameResultLoading = false;
  isNameResultEmpty = false;

  constructor(private userService: UserService, private dialogRef: MatDialogRef<AddShareComponent>) { }

  ngOnInit() {
    this.emailField.valueChanges
      .pipe(
        debounceTime(1000),
        distinctUntilChanged(),
        tap(() => this.isEmailResultLoading = true),
        switchMap((query) => this.userService.searchByEmail(query))
      )
      .subscribe(r => {
        this.emailSearchResults = r;
        this.isEmailResultEmpty = r && r.length === 0;
        this.isEmailResultLoading = false;
      });

    this.nameForm.valueChanges
      .pipe(
        debounceTime(1000),
        distinctUntilChanged(),
        tap(() => this.isNameResultLoading = true),
        switchMap((query) => this.userService.searchByName(query.firstName, query.lastName))
      )
      .subscribe(r => {
        this.nameSearchResults = r;
        this.isNameResultEmpty = r && r.length === 0;
        this.isNameResultLoading = false;
      });
  }

  share(): void {
    // TODO: call sharing service here.
    this.dialogRef.close(true);
  }

}
