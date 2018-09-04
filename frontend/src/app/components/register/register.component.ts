import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, AbstractControl } from '@angular/forms';
import { MatSnackBar } from '@angular/material';

import { User } from '../../models/user';
import { Router } from '@angular/router';
import { LoginService } from '../../services/login.service';

@Component({
  selector: 'wr-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  registrationForm: FormGroup;

  isSubmitting = false;

  constructor(
    private loginService: LoginService,
    private router: Router,
    public snackBar: MatSnackBar) { }

  onSubmit(): void {
    this.isSubmitting = true;

    const user = new User();
    user.firstName = this.registrationForm.controls.firstName.value;
    user.lastName = this.registrationForm.controls.lastName.value;
    user.email = this.registrationForm.controls.email.value;
    user.birthDate = this.registrationForm.controls.birthdate.value;
    user.password = this.registrationForm.controls.password1.value;

    this.loginService.registerUser(user,
      () => {
        this.showMessageInSnack('Registration successful');
        this.router.navigate(['/login']);
      },
      () => {
        this.showMessageInSnack('Registration failed');
      });
  }

  private showMessageInSnack(message: string) {
    this.snackBar.open(message, null, { duration: 1500 });
  }

  ngOnInit() {
    this.registrationForm = new FormGroup({
      firstName: new FormControl('', [Validators.required]),
      lastName: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.required, Validators.email]),
      birthdate: new FormControl(new Date(1990, 0, 1), [this.dateConfirming]),
      password1: new FormControl('', [Validators.required, Validators.minLength(8)]),
      password2: new FormControl('', [Validators.required, Validators.minLength(8)])
    }, [this.passwordConfirming]);
  }

  passwordConfirming(control: AbstractControl): { [key: string]: any } | null {
    return control.get('password1').value !== control.get('password2').value
      ? { 'passwordsmatch': { value: 'pwd' } }
      : null;
  }

  dateConfirming(control: AbstractControl): { [key: string]: any } | null {
    const minDate = new Date(1900, 0, 1);
    const maxDate = new Date();

    return control.value < minDate || control.value > maxDate
      ? { 'outofrange': { value: control.value } }
      : null;
  }

}
