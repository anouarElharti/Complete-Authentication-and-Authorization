import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent {
  users: any = [];

  constructor(private auth: AuthService, private api: ApiService) {}

  ngOnInit() {
    this.api.getAllUsers().subscribe((res) => {
      this.users = res;
    });
  }

  logOut() {
    this.auth.signOut();
  }
}
