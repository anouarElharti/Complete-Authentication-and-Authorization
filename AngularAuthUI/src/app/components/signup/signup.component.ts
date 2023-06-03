import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css'],
})
export class SignupComponent {
  showPass: boolean = false;
  passwordType: string = 'password';
  eyeClass: string = 'fa fa-eye-slash';
  constructor() {}

  ngOnInit(): void {}

  hideShowPass() {
    this.showPass = !this.showPass;
    this.passwordType = this.showPass ? 'text' : 'password';
    this.eyeClass = this.showPass ? 'fa fa-eye' : 'fa fa-eye-slash';
  }
}
