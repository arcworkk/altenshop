import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, map } from 'rxjs';
import { LoginRequest } from './loginRequest.model';
import { AuthResponse } from './authResponse.model';
import { RegisterRequest } from './registerRequest.model';
import { environment } from 'environments/environment';
import { ApiResult } from './apiResult.model';

const TOKEN_KEY = 'access_token';
const ROLE_KEY  = 'user_role';
const EMAIL_KEY = 'user_email';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly API_URL = `${environment.API_BASE_URL}/auth`;

  constructor(private http: HttpClient) {}

  login(loginRequest: LoginRequest): Observable<AuthResponse> {
    return this.http.post<ApiResult<AuthResponse>>(`${this.API_URL}/token`, loginRequest).pipe(
      map(res => {
        if (!res.success || !res.data) throw res.error ?? new Error('Login failed');
        return res.data;
      }),
      tap((authResponse: AuthResponse) => this.setSession(authResponse.email, authResponse.role, authResponse.token))
    );
  }

  register(req: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<ApiResult<AuthResponse>>(`${this.API_URL}/account`, req).pipe(
      map(res => {
        if (!res.success || !res.data) throw res.error ?? new Error('Register failed');
        return res.data;
      }),
      tap((authResponse: AuthResponse) => this.setSession(authResponse.email, authResponse.role, authResponse.token))
    );
  }
  
  private setSession(email: string, role: string, token: string) {
    localStorage.setItem(TOKEN_KEY, token ?? '');
    localStorage.setItem(ROLE_KEY, role ?? '');
    localStorage.setItem(EMAIL_KEY, email ?? '');
  }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(EMAIL_KEY);
    localStorage.removeItem(ROLE_KEY);
  }

  getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  getEmail(): string | null {
    return localStorage.getItem(EMAIL_KEY);
  }

  getRole(): string | null {
    return localStorage.getItem(ROLE_KEY);
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    if(token !== 'undefined' && token !== null)
    {
      return true;
    }
    return false;
  }

  isAdmin(): boolean {
    const role = this.getRole();
    if(role === 'Admin')
    {
      return true;
    }
    return false;
  }
}