import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserStoreService {
  private fullName$ = new BehaviorSubject<string>('');
  private role$ = new BehaviorSubject<string>('');

  constructor() {}

  // GETTER & SETTER FOR ROLE
  public getRoleFromStore() {
    return this.role$.asObservable();
  }
  public setRoleForStore(role: string) {
    this.role$.next(role);
  }
  // GETTER & SETTER FOR FULLNAME
  public getFullNameFromStore() {
    return this.fullName$.asObservable();
  }
  public setFullNameforStore(fullname: string) {
    this.fullName$.next(fullname);
  }
}
