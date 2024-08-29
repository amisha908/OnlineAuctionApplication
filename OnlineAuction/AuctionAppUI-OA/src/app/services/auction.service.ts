import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, of, tap } from 'rxjs';
import { AuctionResult } from '../models/auction.model';
import { throwError } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class AuctionService {
  private endUrl = 'http://localhost:5109/api'; // Backend URL
  private apiUrl = `${this.endUrl}/Auction`;  // Base URL for auction-related APIs

  constructor(private http: HttpClient) {}

  // Method to create an auction
  createAuction(productId: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/create`, { productId });
  }

  // Method to get auction results
  getAuctionResults(productId: string): Observable<AuctionResult[]> {
    return this.http.get<AuctionResult[]>(`${this.endUrl}/Auction/byProduct/${productId}`);
  }
  getAuction(): Observable<AuctionResult[]> {
    return this.http.get<AuctionResult[]>(this.apiUrl).pipe(
      tap(auctions => console.log('Auction fetched:', auctions))
    );
  }
}
