import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { delay, Observable } from 'rxjs';
import { environment } from '../../environments/environment';


export interface AuthRequest {
  username: string;
  password: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) {}

  signIn(data: AuthRequest): Observable<any> {
    return this.http.post(`${this.baseUrl}/api/auth/signin`, data,{ withCredentials: true }).pipe(delay(2000)) ; //delay for 2 seconds to simulate a network delay
  }

  signUp(data: AuthRequest): Observable<any> {
    return this.http.post(`${this.baseUrl}/api/auth/signup`, data,{ withCredentials: true }).pipe(delay(2000));//delay for 2 seconds to simulate a network delay
  }

  logout(): Observable<any> {
    return this.http.post(`${this.baseUrl}/api/auth/logout`, {},{ withCredentials: true });
  }
  
}