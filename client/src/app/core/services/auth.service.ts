import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { ApiService } from './api.service';
import { AuthResponse, LoginRequest, RegisterRequest } from '../models/auth.model';
import { ApiResponse } from '../models/api.model';

const TOKEN_KEY = 'seem_token';
const USER_KEY = 'seem_user';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private currentUserSubject: BehaviorSubject<AuthResponse | null>;
  currentUser$: Observable<AuthResponse | null>;

  constructor(
    private api: ApiService,
    private router: Router,
  ) {
    const stored = localStorage.getItem(USER_KEY);
    const user = stored ? JSON.parse(stored) as AuthResponse : null;
    this.currentUserSubject = new BehaviorSubject<AuthResponse | null>(user);
    this.currentUser$ = this.currentUserSubject.asObservable();
  }

  login(req: LoginRequest): Observable<AuthResponse> {
    return this.api.post<ApiResponse<AuthResponse>>('/auth/login', req).pipe(
      map(response => response.data),
      tap(user => this.storeUser(user)),
    );
  }

  register(req: RegisterRequest): Observable<AuthResponse> {
    return this.api.post<ApiResponse<AuthResponse>>('/auth/register', req).pipe(
      map(response => response.data),
      tap(user => this.storeUser(user)),
    );
  }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  getCurrentUser(): AuthResponse | null {
    return this.currentUserSubject.value;
  }

  private storeUser(user: AuthResponse): void {
    localStorage.setItem(TOKEN_KEY, user.token);
    localStorage.setItem(USER_KEY, JSON.stringify(user));
    this.currentUserSubject.next(user);
  }
}
