import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { LocalstorageService } from './helpers/localstorage.service';
import { LocalstorageKeys } from './helpers/localstoragekeys';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Router } from '@angular/router';
import { PasswordRecovery } from '../models/passwordrecovery';
import { EditUser } from '../models/edituser.model';
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'http://localhost:5190/api/Auth';
  private authState = new BehaviorSubject<boolean>(this.localStorageHelper.hasItem(LocalstorageKeys.TokenName));
  private usernameSubject = new BehaviorSubject<string | null | undefined>(this.localStorageHelper.getItem(LocalstorageKeys.UserId));
  constructor(private localStorageHelper: LocalstorageService, private http: HttpClient,private router :Router) { }
  signUp(user: any): Observable<ApiResponse<string>> {
    const body = user;
    return this.http.post<ApiResponse<string>>(this.apiUrl + '/Register', body)
  }

  signIn(username: string, password: string): Observable<ApiResponse<string>> {
    const body = { username, password };
    return this.http.post<ApiResponse<string>>(this.apiUrl + '/Login', body).pipe(
      tap(response => {
        if (response.success) {
          this.localStorageHelper.setItem(LocalstorageKeys.TokenName, response.data);
          this.localStorageHelper.setItem(LocalstorageKeys.UserId, username);
          this.authState.next(this.localStorageHelper.hasItem(LocalstorageKeys.TokenName));
          this.usernameSubject.next(username);
        }
      })
    )
  }
  isAuthenticated(){
    return this.authState.asObservable();
  }
  SignOut() {
    this.localStorageHelper.removeItem(LocalstorageKeys.TokenName);
    this.localStorageHelper.removeItem(LocalstorageKeys.UserId);
    this.authState.next(false);
    this.usernameSubject.next(null);
    this.router.navigate(['/contacts-pagination']);
  }
  getUsername(): Observable<string | null | undefined> {
    return this.usernameSubject.asObservable();
  }
  forgetpassword(username: string, password: string,confirmPassword:string): Observable<ApiResponse<string>> {
    const body = { username, password,confirmPassword };
    return this.http.post<ApiResponse<string>>(this.apiUrl + "/ForgetPassword", body)
  }
  passwordDiscovery(contact: PasswordRecovery): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(this.apiUrl + '/ForgetPassword', contact);
  }
  fetchUserByloginId(loginId: string|null|undefined): Observable<ApiResponse<EditUser>> {
    return this.http.get<ApiResponse<EditUser>>(this.apiUrl + '/GetUserById/' + loginId);
  }
  editUser(user: EditUser): Observable<ApiResponse<EditUser>> {
    return this.http.put<ApiResponse<EditUser>>(this.apiUrl + '/Edit',user);
  }
}
