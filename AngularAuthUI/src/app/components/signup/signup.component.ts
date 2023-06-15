import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import ValidateForm from 'src/app/helpers/validateForm';
import { AuthService } from 'src/app/services/auth.service';
import { NgToastService } from 'ng-angular-popup';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css'],
})
export class SignupComponent {
  showPass: boolean = false;
  passwordType: string = 'password';
  eyeClass: string = 'fa fa-eye-slash';

  signUpForm!: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private toast: NgToastService
  ) {}

  ngOnInit(): void {
    this.signUpForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      userName: ['', Validators.required],
      email: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  hideShowPass() {
    this.showPass = !this.showPass;
    this.passwordType = this.showPass ? 'text' : 'password';
    this.eyeClass = this.showPass ? 'fa fa-eye' : 'fa fa-eye-slash';
  }

  onSignUp() {
    if (this.signUpForm.valid) {
      // Add logic for signUp
      this.authService.signUp(this.signUpForm.value).subscribe({
        next: (res) => {
          console.log('Register:', res.message);
          this.toast.success({
            detail: 'SUCCESS',
            summary: res.message,
            duration: 3000,
          });
          this.signUpForm.reset();
          this.router.navigate(['login']);
        },
        error: (err) => {
          console.log('Error Registration:', err?.error.message);
          this.toast.error({
            detail: 'ERROR',
            summary: err?.error.message,
            duration: 3000,
          });
        },
      });
      //console.log(this.signUpForm.value);
    } else {
      //Logic for throwing errors
      ValidateForm.validateAllFormFields(this.signUpForm);
    }
  }
}
