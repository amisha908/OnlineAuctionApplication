import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { Product } from '../models/product.model';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = 'http://localhost:5109/api/Product';

  constructor(private http: HttpClient, private authService: AuthService) {}

  addProduct(product: Product): Observable<Product> {
    return this.http.post<Product>(this.apiUrl, product).pipe(
      tap(response => console.log('Product added:', response)),
      catchError(this.handleError)
    );
  }
  getSellerUsername(sellerId: string): Observable<{ username: string }> {
    return this.authService.getUsernameById(sellerId);
  }


  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiUrl).pipe(
      tap(products => console.log('Products fetched:', products)),
      catchError(this.handleError)
    );
  }

  getProductById(productId: string): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/${productId}`).pipe(
      tap(product => console.log('Product fetched:', product)),
      catchError(this.handleError)
    );
  }

  deleteProduct(productId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${productId}`).pipe(
      tap(() => console.log('Product deleted:', productId)),
      catchError(this.handleError)
    );
  }

  private handleError(error: any) {
    console.error('Product Service error', error);
    let errorMessage = 'Product addition failed';
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    return throwError(() => new Error(errorMessage));
  }
}
