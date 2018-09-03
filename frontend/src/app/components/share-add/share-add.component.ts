import { Component, OnInit, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { debounceTime, distinctUntilChanged, switchMap, tap } from 'rxjs/operators';

import { User } from '../../models/user';
import { UserService } from '../../services/user.service';
import { ShareService } from '../../services/share.service';

@Component({
  selector: 'wr-add-share',
  templateUrl: './share-add.component.html',
  styleUrls: ['./share-add.component.scss']
})
export class ShareAddComponent implements OnInit {

  emailFieldPristine = true;

  noteId: string;

  emailField: FormControl = new FormControl();
  nameForm: FormGroup = new FormGroup({ firstName: new FormControl(), lastName: new FormControl() });

  selectedTabIndex = 0;

  selectedByEmail = '';
  selectedByName = '';

  emailSearchResults: User[] = [];
  nameSearchResults: User[] = [];

  addEditInProgress = false;
  addReadInProgress = false;

  isEmailResultLoading = false;
  isEmailResultEmpty = false;
  isNameResultLoading = false;
  isNameResultEmpty = false;

  constructor(
    @Inject(MAT_DIALOG_DATA) data: any,
    private userService: UserService,
    private shareService: ShareService,
    private dialogRef: MatDialogRef<ShareAddComponent>) {
    this.noteId = data.noteId;
  }

  canShare(): boolean {
    switch (this.selectedTabIndex) {
      case 0: return this.selectedByEmail && this.selectedByEmail !== '';
      case 1: return this.selectByName && this.selectedByName !== '';
      default: return false;
    }
  }

  getSelectedUser(): User {
    switch (this.selectedTabIndex) {
      case 0: return this.emailSearchResults.find(x => x.email === this.selectedByEmail);
      case 1: return this.nameSearchResults.find(x => x.email === this.selectedByName);
      default: return null;
    }
  }

  shareToRead(): void {
    this.addReadInProgress = true;
    this.shareService.createShare(this.noteId, this.getSelectedUser(), 1)
      .subscribe(() => this.dialogRef.close(true));
  }

  shareToEdit(): void {
    this.addEditInProgress = true;
    this.shareService.createShare(this.noteId, this.getSelectedUser(), 2)
      .subscribe(() => this.dialogRef.close(true));
  }

  selectByName(user: User): void {
    this.selectedByName = user.email;
  }

  isSelectedByName(user: User): boolean {
    return this.selectedByName === user.email;
  }

  selectByEmail(user: User): void {
    this.selectedByEmail = user.email;
  }

  isSelectedByEmail(user: User): boolean {
    return this.selectedByEmail === user.email;
  }

  ngOnInit() {
    this.emailField.valueChanges
      .pipe(
        debounceTime(750),
        distinctUntilChanged(),
        tap(e => {
          this.isEmailResultLoading = true;
          this.isEmailResultEmpty = true;
          this.selectedByEmail = '';

          if (this.emailFieldPristine && e && e.length) {
            this.emailField.markAsTouched();
            this.emailFieldPristine = false;
           }
        }),
        switchMap((query) => this.userService.searchByEmail(query))
      )
      .subscribe(r => {
        this.emailSearchResults = r;
        this.isEmailResultEmpty = r && r.length === 0;
        this.isEmailResultLoading = false;
      });

    this.nameForm.valueChanges
      .pipe(
        debounceTime(750),
        distinctUntilChanged(),
        tap(() => {
          this.isNameResultLoading = true;
          this.isNameResultEmpty = true;
          this.selectedByName = '';
        }),
        switchMap((query) => this.userService.searchByName(query.firstName, query.lastName))
      )
      .subscribe(r => {
        this.nameSearchResults = r;
        this.isNameResultEmpty = r && r.length === 0;
        this.isNameResultLoading = false;
      });
  }

}
