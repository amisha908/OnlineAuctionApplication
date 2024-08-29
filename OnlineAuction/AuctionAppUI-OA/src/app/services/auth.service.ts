import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5109/api/Auth/login'; // Backend URL
  private authUrl = 'http://localhost:5109/api/Auth/username';
  private tokenSubject = new BehaviorSubject<string | null>(null);
  token$ = this.tokenSubject.asObservable();
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(private http: HttpClient) {
    const savedToken = localStorage.getItem('authToken');
    if (savedToken) {
      this.tokenSubject.next(savedToken);
      this.isAuthenticatedSubject.next(true);
    }
  }

  login(email: string, password: string): Observable<any> {
    return this.http.post<any>(this.apiUrl, { email, password }).pipe(
      tap(response => {
        if (response.token) {
          localStorage.setItem('authToken', response.token);
          this.tokenSubject.next(response.token);
          this.isAuthenticatedSubject.next(true);
          console.log('Token:', response.token); // Print the token to the console
        }
      }),
      catchError(this.handleError)
    );
  }

  logout(): void {
    localStorage.removeItem('authToken');
    this.tokenSubject.next(null);
    this.isAuthenticatedSubject.next(false);
  }

  getRoleFromToken(): string | null {
    const token = localStorage.getItem('authToken');
    if (token) {
      const payload = JSON.parse(atob(token.split('.')[1])); // Decode JWT token
      return payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || null;
    }
    return null;
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('authToken');
  }

  private handleError(error: any) {
    console.error('Login error', error);
    return throwError(() => new Error('Login failed'));
  }
  // Add the following method to the AuthService class

getUserIdFromToken(): string {
  const token = localStorage.getItem('authToken');
  if (token) {
    const payload = JSON.parse(atob(token.split('.')[1])); // Decode JWT token
    return payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || null;
  }
  return "";
}
getUsernameById(userId: string): Observable<{ username: string }> {
  return this.http.get<{ username: string }>(`${this.authUrl}/${userId}`).pipe(
    tap(response => console.log('Username fetched:', response.username)),
    catchError(this.handleError)
  );
}
}
