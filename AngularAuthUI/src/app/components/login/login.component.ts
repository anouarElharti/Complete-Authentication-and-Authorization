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
import { UserStoreService } from 'src/app/services/user-store.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  showPass: boolean = false;
  passwordType: string = 'password';
  eyeClass: string = 'fa fa-eye-slash';

  loginForm!: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private toast: NgToastService,
    private userStore: UserStoreService
  ) {}

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  hideShowPass() {
    this.showPass = !this.showPass;
    this.passwordType = this.showPass ? 'text' : 'password';
    this.eyeClass = this.showPass ? 'fa fa-eye' : 'fa fa-eye-slash';
  }

  onSubmit() {
    if (this.loginForm.valid) {
      // send the object to the api
      this.authService.login(this.loginForm.value).subscribe({
        next: (res) => {
          console.log('Response:', res.message);
          // STORE THE TOKEN
          this.authService.storeToken(res.token);
          // SETTING THE USER NAME FROM TOKEN & ROLE
          let tokenPayload = this.authService.decodedToken();
          this.userStore.setFullNameforStore(tokenPayload.unique_name);
          this.userStore.setRoleForStore(tokenPayload.role);
          // TOAST ALERT MESSAGE
          this.toast.success({
            detail: 'SUCCESS',
            summary: res.message,
            duration: 3000,
          });
          this.loginForm.reset();
          this.router.navigate(['dashboard']);
        },
        error: (err) => {
          console.log('Error:', err?.error.message);
          this.toast.error({
            detail: 'ERROR',
            summary: err.error.message,
            duration: 3000,
          });
        },
      });
      //console.log(this.loginForm.value);
    } else {
      // throw error using toaster
      ValidateForm.validateAllFormFields(this.loginForm);
      console.log('Form not valid');
    }
  }
}
