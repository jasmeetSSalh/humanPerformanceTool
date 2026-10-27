import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl = '/api/auth';

  // The signed-in instructor's username, or null when signed out.
  readonly username = signal<string | null>(null);

  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<void> {
    return this.http
      .post<{ username: string }>(`${this.baseUrl}/login`, { username, password })
      .pipe(
        tap(response => this.username.set(response.username)),
        map(() => undefined)
      );
  }

  logout(): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/logout`, {}).pipe(
      tap(() => this.username.set(null))
    );
  }

  // Asks the API whether the session cookie is still valid.
  checkSession(): Observable<boolean> {
    if (this.username()) {
      return of(true);
    }
    return this.http.get<{ username: string }>(`${this.baseUrl}/me`).pipe(
      tap(response => this.username.set(response.username)),
      map(() => true),
      catchError(() => of(false))
    );
  }

  clearSession(): void {
    this.username.set(null);
  }
}
