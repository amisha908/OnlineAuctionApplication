import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Bid } from '../models/bid.model';

@Injectable({
  providedIn: 'root'
})
export class BidService {
  private apiUrl = 'http://localhost:5109/api/Bid';

  constructor(private http: HttpClient) { }

  placeBid(bid: Bid): Observable<Bid> {
    return this.http.post<Bid>(this.apiUrl, bid);
  }
  getBidsByProductId(productId: string): Observable<Bid[]> {
    return this.http.get<Bid[]>(`${this.apiUrl}/product/${productId}`);
  }
}
