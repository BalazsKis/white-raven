import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { LoginService } from '../../services/login.service';

@Component({
  selector: 'wr-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  loginForm: FormGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(8)])
  });

  isSubmitting = false;
  hasLoginError = false;
  loginError = '';

  constructor(
    private loginService: LoginService,
    private router: Router) { }

  onSubmit() {
    this.isSubmitting = true;

    this.loginService.login(this.loginForm.controls.email.value, this.loginForm.controls.password.value,
      () => {
        this.isSubmitting = false;
        this.router.navigate(['/app']);
      },
      error => {
        this.isSubmitting = false;
        this.hasLoginError = true;
        switch (error.status) {
          case 403:
            this.loginError = 'The password is incorrect';
            break;
          case 404:
            this.loginError = 'We can\'t find a user with this email address';
            break;
          default:
            this.loginError = 'An unknown error happened while we tried to sign you in';
            console.log(error);
            break;
        }
      });
  }

}
