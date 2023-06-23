import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { UserStoreService } from 'src/app/services/user-store.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent {
  users: any = [];

  private fullName: string = '';
  private role: string = '';

  constructor(
    private auth: AuthService,
    private api: ApiService,
    private userStore: UserStoreService
  ) {}

  ngOnInit() {
    this.api.getAllUsers().subscribe((res) => {
      this.users = res;
    });
    this.userStore.getFullNameFromStore().subscribe((val) => {
      let fullNameFromToken = this.auth.getFullNameFromToken();
      this.fullName = val || fullNameFromToken;
    });
  }

  logOut() {
    this.auth.signOut();
  }
}
